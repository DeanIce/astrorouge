using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphaWolf : BasicEnemyAgent
{
    [SerializeField] private List<GameObject> wolves;
    [SerializeField] private float attackChance = 0.5f;
    private Animator animator;
    private int attack;
    private bool condition;
    private const float viewAngle = 30f;
    private Rigidbody rb;

    public override void Start()
    {
        animator = GetComponentInChildren<Animator>();
        animator.SetInteger("moving", 1);
        Dying = false;
        rb = GetComponent<Rigidbody>();
        attack = 0;
        base.Start();
    }

    public override void FixedUpdate()
    {
        Detector.transform.localScale = new Vector3(8 + wolves.Count, 1, 8 + wolves.Count);

        foreach (GameObject wolf in wolves)
        {
            wolf.GetComponent<Wolf>().maxDistance = 5 + wolves.Count;
        }

        if (Wandering)
        {
            attack = 0;
        }

        base.FixedUpdate();
    }

    public override void Hunt(Collider target)
    {
        RaycastHit[] hits;

        DoGravity();

        foreach (GameObject wolf in wolves)
        {
            if (Random.value < attackChance || wolf.GetComponent<Wolf>().ordered == true) wolf.GetComponent<Wolf>().ReceiveOrder(target);
        }

        //attacking
        hits = Physics.RaycastAll(transform.position, Body.transform.forward, AttackRange, LayerMask.GetMask("Player"));
        if (hits.Length != 0)
        {
            //check for the player in the things the ray hit by whether it has a PlayerDefault
            foreach (var hit in hits)
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
            if (wolves.Count > 0) condition = !Attacking && (Mathf.Abs((transform.position - target.transform.position).magnitude) > AttackRange + wolves.Count && Vector3.Angle(Body.transform.forward, target.transform.position - Body.transform.position) > viewAngle);
            else condition = !Attacking;

            if (condition)
            {
                // NEW MOVEMENT HERE
                animator.SetInteger("moving", 2);
                rb.MovePosition(
                    rb.position + (target.transform.position - rb.position) * Time.deltaTime * movementSpeed);
                Body.transform.RotateAround(transform.position, transform.up,
                    -Vector3.SignedAngle(target.transform.position - transform.position, Body.transform.forward,
                        transform.up) /
                    10);

                // Jumping - commented as only works in 2d but could bring back if desired?
                //if (targetRb.transform.position.y > transform.position.y && IsGrounded()) Jump();
            }
            else
            {
                animator.SetInteger("moving", 0);
            }
        }
    }

    public override IEnumerator Attack()
    {
        //rend.enabled = true;
        Attacking = true;
        StartCoroutine(AttackAnim());
        if (attack != 2) yield return new WaitForSeconds(1.25f);
        else yield return new WaitForSeconds(1.458f);
        if (animator.GetInteger("battle") == 1) animator.SetInteger("moving", 2);
        else animator.SetInteger("moving", 1);
        //rend.enabled = false;
        Attacking = false;
        attack += 1;
        if (attack > 2) attack = 0;
    }

    public override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == PlayerLayer)
        {
            StartCoroutine(BattleAnim(true));
        }
        base.OnTriggerEnter(other);
    }

    public override void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == PlayerLayer)
        {
            StartCoroutine(BattleAnim(false));
            foreach (GameObject wolf in wolves)
            {
                wolf.GetComponent<Wolf>().OnTriggerExit(other);
            }
        }
        base.OnTriggerExit(other);
    }

    public override void Die()
    {
        if (!Dying)
        {
            Dying = true;
            foreach (GameObject wolf in wolves)
            {
                wolf.GetComponent<Wolf>().SetAlpha(null);
            }
            if (Random.value < 0.5) StartCoroutine(DeathAnim(13));
            else StartCoroutine(DeathAnim(12));
            base.Die();
        }
    }

    private IEnumerator DeathAnim(int anim)
    {
        animator.SetInteger("moving", anim);
        yield return new WaitForSeconds(0.05f);
        animator.SetInteger("moving", 0);
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
        if (attack == 0)
        {
            animator.SetInteger("moving", 3);
        }
        else if (attack == 1)
        {
            animator.SetInteger("moving", 4);
        }
        else if (attack == 2)
        {
            animator.SetInteger("moving", 6);
        }
    }

    public void AddWolf (GameObject wolf)
    {
        wolves.Add(wolf);
    }

    public void RemoveWolf(GameObject wolf)
    {
        wolves.Remove(wolf);
    }
}
