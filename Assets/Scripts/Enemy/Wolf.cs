using System.Collections;
using Managers;
using UnityEngine;

public class Wolf : BasicEnemyAgent
{
    [SerializeField] private AlphaWolf alpha;
    public float maxDistance = 5f;
    public bool ordered;
    private Animator animator;
    private bool attacked;

    public override void Start()
    {
        health *= (3 * LevelSelect.Instance.requestedLevel + 1);
        maxHealth = health;
        animator = GetComponentInChildren<Animator>();
        animator.SetInteger("moving", 1);
        if (alpha != null) alpha.AddWolf(gameObject);
        Dying = false;
        attacked = false;
        ordered = false;
        base.Start();
    }

    public override void FixedUpdate()
    {
        Detector.GetComponent<Collider>().enabled = alpha == null;
        CheckDeath();
        base.FixedUpdate();
    }

    public override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == PlayerLayer) StartCoroutine(BattleAnim(true));
        base.OnTriggerEnter(other);
    }

    public override void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == PlayerLayer) StartCoroutine(BattleAnim(false));
        base.OnTriggerExit(other);
    }

    public override void OnTriggerStay(Collider other)
    {
        if (Wandering) base.OnTriggerEnter(other);
        base.OnTriggerStay(other);
    }

    public override void Wander(Vector3 direction)
    {
        DoGravity();

        if (alpha != null && Mathf.Abs((transform.position - alpha.transform.position).magnitude) > maxDistance)
            base.Wander(alpha.transform.position - Body.transform.position);
        else base.Wander(direction);
    }

    public override IEnumerator Attack()
    {
        //rend.enabled = true;
        Attacking = true;
        StartCoroutine(AttackAnim());
        yield return WaitForSecondsOrDie(0.833f/animator.speed);
        animator.speed = 1;
        if (animator.GetInteger("battle") == 1) animator.SetInteger("moving", 2);
        else animator.SetInteger("moving", 1);
        //rend.enabled = false;
        Attacking = false;
        attacked = true;
    }

    public override void Die()
    {
        if (!Dying)
        {
            Dying = true;
            if (alpha != null) alpha.GetComponent<AlphaWolf>().RemoveWolf(gameObject);
            animator.SetInteger("moving", 12);
            base.Die();
        }
    }

    public void SetAlpha(AlphaWolf boss)
    {
        alpha = boss;
        if (alpha != null) alpha.AddWolf(gameObject);
    }

    private void CheckDeath()
    {
        if (Dying && (animator.GetInteger("moving") != 13 && animator.GetInteger("moving") != 12))
        {
            animator.SetInteger("moving", 12);
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
        animator.SetInteger("moving", 3);
    }

    public void ReceiveOrder(Collider target)
    {
        if (!ordered) OnTriggerEnter(target);
        ordered = true;
        Hunt(target);
        if (attacked)
        {
            ordered = false;
            attacked = false;
            OnTriggerExit(target);
        }
    }
}