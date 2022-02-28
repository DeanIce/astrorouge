using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerEnemy : MeleeEnemy
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
        animator.SetInteger("moving", 3);
        yield return new WaitForSeconds(0.7f);
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
        if (Random.value < 0.5) animator.SetInteger("moving", 13);
        else animator.SetInteger("moving", 12);
        yield return new WaitForSecondsRealtime(10);
        Destroy(gameObject);
    }
}