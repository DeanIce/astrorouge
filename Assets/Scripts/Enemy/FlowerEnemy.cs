using System.Collections;
using UnityEngine;

public class FlowerEnemy : BasicEnemyAgent
{
    private Animator animator;

    public override void Start()
    {
        health *= (Managers.LevelSelect.Instance.requestedLevel + 1);
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
            if (Random.value < 0.5) StartCoroutine(DeathAnim(12));
            else StartCoroutine(DeathAnim(13));
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