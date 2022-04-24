using System.Collections;
using Managers;
using UnityEngine;

public class BatEnemy : BasicEnemyAgent
{
    private Animator animator;

    public override void Start()
    {
        health *= 2 * (LevelSelect.Instance.requestedLevel + 1);
        maxHealth = health;
        animator = GetComponentInChildren<Animator>();
        animator.SetInteger("battle", 1);
        Dying = false;
        base.Start();
    }

    public override void FixedUpdate()
    {
        CheckDeath();
        base.FixedUpdate();
    }

    public override IEnumerator Attack()
    {
        //rend.enabled = true;
        Attacking = true;
        animator.speed = 2;
        animator.SetInteger("moving", 2);
        yield return WaitForSecondsOrDie(2.08f/animator.speed);
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
            animator.SetInteger("moving", 14);
            base.Die();
        }
    }

    private void CheckDeath()
    {
        if (Dying && (animator.GetInteger("moving") != 13 && animator.GetInteger("moving") != 12))
        {
            animator.SetInteger("moving", 14);
        }
    }
}