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
        animator.SetInteger("battle", 1);
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
        animator.SetInteger("moving", 3);
        yield return new WaitForSeconds(0.7f);
        animator.SetInteger("moving", 0);
        //rend.enabled = false;
        Attacking = false;
    }

    public override void Die()
    {
        if (!Dying)
        {
            Dying = true;
            if (Random.value < 0.5) animator.SetInteger("moving", 13);
            else animator.SetInteger("moving", 12);
            base.Die();
        }
    }

    public void SetAlpha(AlphaWolf boss)
    {
        alpha = boss;
    }
}
