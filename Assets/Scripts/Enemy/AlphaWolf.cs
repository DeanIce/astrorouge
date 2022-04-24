using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;

public class AlphaWolf : BasicEnemyAgent
{
    private const float viewAngle = 30f;
    [SerializeField] private List<GameObject> wolves;
    [SerializeField] private float attackChance = 0.5f;
    [SerializeField] private GameObject wolf;
    private Animator animator;
    private int attack, wolfNum;
    private bool condition;
    private float rand;
    private Rigidbody rb2;

    public override void Start()
    {
        health *= 2 * (LevelSelect.Instance.requestedLevel + 1);
        maxHealth = health;
        animator = GetComponentInChildren<Animator>();
        animator.SetInteger("moving", 1);
        Dying = false;
        rb2 = GetComponent<Rigidbody>();
        attack = 0;
        rand = Random.value;
        if (rand < 0.1) wolfNum = 0;
        else if (rand < 0.2) wolfNum = 1;
        else if (rand < 0.55) wolfNum = 2;
        else if (rand < 0.9) wolfNum = 3;
        else wolfNum = 4;
        for (var count = 0; count < wolfNum; count++)
        {
            GameObject enemy = Instantiate(wolf);
            enemy.transform.position = transform.position;
            enemy.tag = "enemy";
            enemy.GetComponent<Wolf>().SetAlpha(this);
            AddWolf(enemy);
        }

        base.Start();
    }

    public override void FixedUpdate()
    {
        Detector.transform.localScale = new Vector3(30 + wolves.Count, 15, 30 + wolves.Count);

        foreach (GameObject wolf in wolves)
        {
            if (wolf != null)
            {
                if (wolf && wolf.GetComponent<Wolf>())
                    wolf.GetComponent<Wolf>().maxDistance = 5 + wolves.Count;
            }
        }

        CheckDeath();

        if (Wandering) attack = 0;

        base.FixedUpdate();
    }

    public override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == PlayerLayer) StartCoroutine(BattleAnim(true));
        base.OnTriggerEnter(other);
    }

    public override void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == PlayerLayer)
        {
            StartCoroutine(BattleAnim(false));
            foreach (GameObject wolf in wolves)
            {
                if (wolf != null)
                    wolf.GetComponent<Wolf>().OnTriggerExit(other);
            }
        }

        base.OnTriggerExit(other);
    }

    public override void Hunt(Collider target)
    {
        RaycastHit[] hits;

        DoGravity();

        foreach (GameObject wolf in wolves)
        {
            if (wolf != null)
            {
                if (Random.value < attackChance || wolf.GetComponent<Wolf>().ordered)
                    wolf.GetComponent<Wolf>().ReceiveOrder(target);
            }
        }

        //attacking
        hits = Physics.RaycastAll(transform.position, Body.transform.forward, AttackRange, LayerMask.GetMask("Player"));
        if (hits.Length != 0)
        {
            //check for the player in the things the ray hit by whether it has a PlayerDefault
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.gameObject.GetComponent<PlayerDefault>() != null)
                {
                    //it makes more sense of the !attacking condition to just be above but for some reason it doesn't work there
                    if (!Attacking && Health > 0) StartCoroutine(Attack());
                    break;
                }
            }
        }
        else
        {
            if (wolves.Count > 0)
            {
                condition = !Attacking &&
                            (Mathf.Abs((Body.transform.position - target.transform.position).magnitude) >
                                AttackRange + wolves.Count || Vector3.Angle(Body.transform.forward,
                                    target.transform.position - Body.transform.position) > viewAngle);
            }
            else condition = !Attacking;

            if (condition)
            {
                // NEW MOVEMENT HERE
                animator.SetInteger("moving", 2);
                rb2.MovePosition(
                    rb2.position + (target.transform.position - rb2.position) * Time.deltaTime * movementSpeed);
                Body.transform.RotateAround(transform.position, transform.up,
                    -Vector3.SignedAngle(target.transform.position - transform.position, Body.transform.forward,
                        transform.up) /
                    10);

                // Jumping - commented as only works in 2d but could bring back if desired?
                //if (targetRb.transform.position.y > transform.position.y && IsGrounded()) Jump();
            }
            else
                animator.SetInteger("moving", 0);
        }
    }

    public override IEnumerator Attack()
    {
        //rend.enabled = true;
        Attacking = true;
        StartCoroutine(AttackAnim());
        if (attack != 2) yield return WaitForSecondsOrDie(1.25f / animator.speed);
        else yield return WaitForSecondsOrDie(1.458f / animator.speed);
        animator.speed = 1;
        if (animator.GetInteger("battle") == 1) animator.SetInteger("moving", 2);
        else animator.SetInteger("moving", 1);
        //rend.enabled = false;
        Attacking = false;
        attack += 1;
        if (attack > 2) attack = 0;
    }

    public override void Die()
    {
        if (!Dying)
        {
            Dying = true;
            foreach (GameObject wolf in wolves)
            {
                if (wolf != null)
                    wolf.GetComponent<Wolf>().SetAlpha(null);
            }

            if (Random.value < 0.5) animator.SetInteger("moving", 12);
            else animator.SetInteger("moving", 13);
            base.Die();
        }
    }

    private void CheckDeath()
    {
        if (Dying && animator.GetInteger("moving") != 13 && animator.GetInteger("moving") != 12)
        {
            if (Random.value < 0.5) animator.SetInteger("moving", 12);
            else animator.SetInteger("moving", 13);
        }
    }

    private IEnumerator BattleAnim(bool start)
    {
        animator.SetInteger("moving", 0);
        yield return new WaitForSeconds(0.05f);
        if (start)
        {
            animator.SetInteger("battle", 1);
            animator.SetInteger("moving", 2);
        }
        else
        {
            animator.SetInteger("battle", 0);
            animator.SetInteger("moving", 1);
        }
    }

    private IEnumerator AttackAnim()
    {
        animator.SetInteger("moving", 0);
        yield return new WaitForSeconds(0.05f);
        animator.speed = 2;
        if (attack == 0)
            animator.SetInteger("moving", 3);
        else if (attack == 1)
            animator.SetInteger("moving", 4);
        else if (attack == 2) animator.SetInteger("moving", 6);
    }

    public void AddWolf(GameObject wolf)
    {
        wolves.Add(wolf);
    }

    public void RemoveWolf(GameObject wolf)
    {
        wolves.Remove(wolf);
    }
}