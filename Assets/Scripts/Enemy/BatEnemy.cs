using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatEnemy: MeleeEnemy
{
    Animator animator;

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
        yield return new WaitForSeconds(1f);
        animator.SetInteger("moving", 0);
        //rend.enabled = false;
        Attacking = false;
    }

    public override void Die()
    {
        if (!Dying) StartCoroutine(DieCo());
    }

    private IEnumerator DieCo()
    {
        Dying = true;
        animator.SetInteger("moving", 14);
        yield return new WaitForSecondsRealtime(10);
        Destroy(gameObject);
    }
}