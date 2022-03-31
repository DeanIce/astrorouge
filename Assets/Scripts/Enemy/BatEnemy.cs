using System.Collections;
using UnityEngine;

public class BatEnemy : BasicEnemyAgent
{
    private Animator animator;

    public override void Start()
    {
        animator = GetComponentInChildren<Animator>();
        animator.SetInteger("battle", 1);
        Dying = false;
        base.Start();
    }

    public override IEnumerator Attack()
    {
        //rend.enabled = true;
        Attacking = true;
        animator.SetInteger("moving", 2);
        yield return new WaitForSeconds(2.08f);
        animator.SetInteger("moving", 0);
        //rend.enabled = false;
        Attacking = false;
    }

    public override void Die()
    {
        if (!Dying)
        {
            Dying = true;
            animator.SetInteger("moving", 14);
            base.Die();
        }
    }
}