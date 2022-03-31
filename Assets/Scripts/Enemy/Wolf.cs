using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : BasicEnemyAgent
{
    [SerializeField] private AlphaWolf alpha;
    public float maxDistance = 5f;
    private bool attacked;
    public bool ordered;
    private Animator animator;

    public override void Start()
    {
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
        Detector.GetComponent<Collider>().enabled = (alpha == null);
        base.FixedUpdate();
    }

    public override void Wander(Vector3 direction)
    {
        DoGravity();

        if (alpha != null && Mathf.Abs((transform.position - alpha.transform.position).magnitude) > maxDistance) base.Wander(alpha.transform.position - Body.transform.position);
        else base.Wander(direction);
    }

    public override void Hunt(Collider target)
    {
        if (alpha == null)
        {
            base.Hunt(target);
        }
    }

    public override IEnumerator Attack()
    {
        //rend.enabled = true;
        Attacking = true;
        StartCoroutine(AttackAnim());
        yield return new WaitForSeconds(0.833f);
        if (animator.GetInteger("battle") == 1) animator.SetInteger("moving", 2);
        else animator.SetInteger("moving", 1);
        //rend.enabled = false;
        Attacking = false;
        attacked = true;
    }

    public override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == PlayerLayer)
        {
            StartCoroutine(BattleAnim(true));            
        }
        base.OnTriggerEnter(other);
    }

    public override void OnTriggerStay(Collider other)
    {
        if (Wandering) base.OnTriggerEnter(other);
        base.OnTriggerStay(other);
    }

    public override void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == PlayerLayer)
        {
            StartCoroutine(BattleAnim(false));            
        }
        base.OnTriggerExit(other);
    }

    public override void Die()
    {
        if (!Dying)
        {
            Dying = true;
            if (alpha != null)
            {
                alpha.GetComponent<AlphaWolf>().RemoveWolf(gameObject);
            }
            animator.SetInteger("moving", 12);
            base.Die();
        }
    }

    public void SetAlpha(AlphaWolf boss)
    {
        alpha = boss;
        if (alpha != null) alpha.AddWolf(gameObject);
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
        animator.SetInteger("moving", 3);
    }

    public void ReceiveOrder(Collider target)
    {
        ordered = true;
        base.Hunt(target);
        if (attacked)
        {
            ordered = false;
            attacked = false;
        }
    }
}
