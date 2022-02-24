using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockEnemy : MeleeEnemy
{
    private int attack;
    private bool started;
    
    Animator animator;

    public override void Start()
    {
        animator = GetComponentInChildren<Animator>();
        Dying = false;
        started = false;
        attack = 0;
        base.Start();
    }

    public override void FixedUpdate()
    {
        if (Wandering) attack = 0;
        if (started) base.FixedUpdate();
        else DoGravity();
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

    public override void OnTriggerEnter(Collider other)
    {
        if (!started && other.gameObject.layer == LayerMask.NameToLayer("Player")) StartCoroutine(Appear(other));
        else base.OnTriggerEnter(other);
    }

    public override void OnTriggerStay(Collider other)
    {
        if (started)
        {
            base.OnTriggerEnter(other);
            base.OnTriggerStay(other);
        }
    }

    public override void Die()
    {
        if (!Dying) StartCoroutine(DieCo());
    }

    private IEnumerator DieCo()
    {
        Dying = true;
        animator.SetBool("attack1A", false);
        animator.SetBool("death", true);
        yield return new WaitForSecondsRealtime(10);
        Destroy(gameObject);
    }

    private IEnumerator Appear(Collider other)
    {
        animator.SetBool("rubbleToIdle", true);
        yield return new WaitForSecondsRealtime(5);
        started = true;
    }
}
