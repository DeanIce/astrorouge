using System.Collections;
using Managers;
using UnityEngine;

public class BatEnemy : BasicEnemyAgent
{
    private Animator animator;

    public override void Start()
    {
        health *= LevelSelect.Instance.requestedLevel + 1;
        maxHealth = health;
        animator = GetComponentInChildren<Animator>();
        animator.SetInteger("battle", 1);
        Dying = false;
        base.Start();
    }

    public override IEnumerator Attack()
    {
        //rend.enabled = true;
        Attacking = true;
        animator.speed = 2;
        animator.SetInteger("moving", 2);
        yield return WaitForSecondsOrDie(2.08f);
        animator.speed = 1;
        animator.SetInteger("moving", 0);
        //rend.enabled = false;
        Attacking = false;
    }

    public override void Die()
    {
        if (!Dying)
        {
            Dying = true;
            StartCoroutine(DeathAnim(14));
            base.Die();
        }
    }

    private IEnumerator DeathAnim(int anim)
    {
        yield return new WaitForSeconds(0.2f);
        animator.SetInteger("moving", anim);
        yield return new WaitForSeconds(0.1f);
        animator.SetInteger("moving", 0);
    }
}