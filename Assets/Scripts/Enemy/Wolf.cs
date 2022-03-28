using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : BasicEnemyAgent
{
    [SerializeField] private AlphaWolf alpha;
    private Animator animator;

    public override void Start()
    {
        animator = GetComponentInChildren<Animator>();
        animator.SetInteger("moving", 1);
        Dying = false;
        base.Start();
    }

    public override void Hunt(Collider target)
    {
        if (alpha == null)
        {
            base.Hunt(target);
        }
        else
        {
            alpha.Alert(target);
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
        }
        base.OnTriggerExit(other);
    }

    public override void Die()
    {
        if (!Dying)
        {
            Dying = true;
            animator.SetInteger("moving", 12);
            base.Die();
        }
    }

    public void SetAlpha(AlphaWolf boss)
    {
        alpha = boss;
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
}
