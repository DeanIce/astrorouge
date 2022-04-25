using System.Collections;
using Managers;
using UnityEngine;

public class FlowerEnemy : BasicEnemyAgent
{
    private Animator animator;

    public override void Start()
    {
        health *= (2 * LevelSelect.Instance.requestedLevel + 1);
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
        animator.SetInteger("moving", 3);
        yield return WaitForSecondsOrDie(0.833f/animator.speed);
        animator.SetInteger("moving", 0);
        animator.speed = 1;
        //rend.enabled = false;
        Attacking = false;
    }

    public override void Die()
    {
        if (!Dying)
        {
            Dying = true;
            if (Random.value < 0.5) animator.SetInteger("moving", 12);
            else animator.SetInteger("moving", 13);
            base.Die();
        }
    }

    private void CheckDeath()
    {
        if (Dying && (animator.GetInteger("moving") != 13 && animator.GetInteger("moving") != 12))
        {
            if (Random.value < 0.5) animator.SetInteger("moving", 12);
            else animator.SetInteger("moving", 13);
        }
    }
}