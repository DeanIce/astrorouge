using System.Collections;
using UnityEngine;

public class RockEnemy : BasicEnemyAgent
{
    private Animator animator;
    private int attack;
    private bool started, appear;

    public override void Start()
    {
        animator = GetComponentInChildren<Animator>();
        Dying = false;
        started = false;
        appear = false;
        attack = 0;
        base.Start();
    }

    public override void FixedUpdate()
    {
        if (Wandering)
        {
            attack = 0;
        }

        if (started)
        {
            base.FixedUpdate();
        }
        else
        {
            DoGravity();
        }
    }

    public override void OnTriggerEnter(Collider other)
    {
        if (!started && other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (!appear) StartCoroutine(Appear(other));
        }
        else
        {
            base.OnTriggerEnter(other);
        }
    }

    public override void OnTriggerStay(Collider other)
    {
        if (started)
        {
            base.OnTriggerEnter(other);
            base.OnTriggerStay(other);
        }
    }

    public override IEnumerator Attack()
    {
        //rend.enabled = true;
        Attacking = true;
        if (attack == 0)
        {
            animator.SetBool("attack1A", true);
            yield return new WaitForSeconds(2f);
        }
        else if (attack == 1)
        {
            animator.SetBool("attack1B", true);
            yield return new WaitForSeconds(2f);
        }
        else if (attack == 2)
        {
            animator.SetBool("attack2", true);
            yield return new WaitForSeconds(2.4f);
        }

        //rend.enabled = false;
        Attacking = false;
        if (attack == 0)
        {
            animator.SetBool("attack1A", false);
            attack = 1;
        }
        else if (attack == 1)
        {
            animator.SetBool("attack1B", false);
            attack = 2;
        }
        else if (attack == 2)
        {
            animator.SetBool("attack2", false);
            attack = 0;
        }
    }

    public override void Die()
    {
        if (!Dying)
        {
            Dying = true;
            animator.SetBool("death", true);
            // StartCoroutine(DieCo());
            base.Die();
        }
    }


    private IEnumerator Appear(Collider other)
    {
        appear = true;
        animator.SetBool("rubbleToIdle", true);
        yield return new WaitForSecondsRealtime(5);
        started = true;
    }
}