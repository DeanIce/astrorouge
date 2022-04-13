using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidEnt : BasicEnemyAgent
{
    private Animator animator;
    private float summonChance = 0.1f;
    private int attack;
    private bool summon;
    [SerializeField] private GameObject flower;

    public override void Start()
    {
        health *= (Managers.LevelSelect.Instance.requestedLevel + 1);
        animator = GetComponentInChildren<Animator>();
        animator.SetInteger("battle", 1);
        animator.SetInteger("moving", 2);
        Dying = false;
        attack = 0;
        summon = false;
        base.Start();
    }

    public override void FixedUpdate()
    {
        if (Wandering)
        {
            attack = 0;
        }

        base.FixedUpdate();
    }

    public override IEnumerator Attack()
    {
        //rend.enabled = true;
        Attacking = true;
        StartCoroutine(BattleAnim());
        if (attack == 2) yield return WaitForSecondsOrDie(2.708f);
        else if (summon) yield return WaitForSecondsOrDie(1.667f);
        else yield return WaitForSecondsOrDie(0.833f);
        if (summon)
        {
            GameObject enemy = Instantiate(flower);
            enemy.transform.position = transform.position + 2*Body.transform.forward;
            enemy.tag = "enemy";
        }
        animator.speed = 1;
        animator.SetInteger("moving", 2);
        //rend.enabled = false;
        Attacking = false;
        if (!summon) attack += 1;
        if (attack > 2) attack = 0;
        summon = false;
    }

    private IEnumerator BattleAnim()
    {
        animator.SetInteger("moving", 0);
        yield return new WaitForSeconds(0.05f);
        if (attack == 0)
        {
            if (Random.value < summonChance)
            {
                animator.SetInteger("moving", 7);
                summon = true;
            }
            else
            {
                animator.speed = 2;
                animator.SetInteger("moving", 3);
            }
        }
        else if (attack == 1)
        {
            animator.speed = 2;
            animator.SetInteger("moving", 4);
        }
        else if (attack == 2)
        {
            animator.speed = 2;
            animator.SetInteger("moving", 6);
        }
    }

    public override void Die()
    {
        if (!Dying)
        {
            Dying = true;
            if (Random.value < 0.5) StartCoroutine(DeathAnim(13));
            else StartCoroutine(DeathAnim(14));
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
