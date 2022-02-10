using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeEnemy : RangedEnemy
{
    Animator animator;
    ProjectileFactory factory;

    public override void Start()
    {
        Dying = false;
        animator = GetComponentInChildren<Animator>();
        factory = ProjectileFactory.Instance;
        base.Start();
    }

    public override IEnumerator Attack()
    {
        RaycastHit[] hits;
        //rend.enabled = true;
        Attacking = true;
        animator.SetBool("attack3", true);
        yield return new WaitForSeconds(2f);
        factory.CreateBasicProjectile(transform.position, transform.forward, LayerMask.GetMask("Player", "Ground"), 5, 5);
        //rend.enabled = false;
        Attacking = false;
        animator.SetBool("attack3", false);
    }

    public override void Die()
    {
        if (!Dying) StartCoroutine(DieCo());
    }

    private IEnumerator DieCo()
    {
        Dying = true;
        animator.SetBool("attack3", false);
        animator.SetBool("death", true);
        yield return new WaitForSecondsRealtime(10);
        Destroy(gameObject);
    }
}
