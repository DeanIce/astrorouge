using System.Collections;
using Managers;
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
        health *= (3 * LevelSelect.Instance.requestedLevel + 1);
        maxHealth = health;
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

        CheckDeath();

        base.FixedUpdate();
    }

    public override IEnumerator Attack()
    {
        //rend.enabled = true;
        Attacking = true;
        StartCoroutine(BattleAnim());
        if (attack == 2) yield return WaitForSecondsOrDie(2.708f/animator.speed);
        else if (summon) yield return WaitForSecondsOrDie(1.667f);
        else yield return WaitForSecondsOrDie(0.833f/animator.speed);
        if (summon)
        {
            GameObject enemy = Instantiate(flower);
            enemy.transform.position = transform.position + 2*Body.transform.up + 2*Body.transform.forward;
            enemy.tag = "enemy";
        }
        animator.speed = 1;
        animator.SetInteger("moving", 2);
        //rend.enabled = false;
        if (!summon) attack += 1;
        if (attack > 2) attack = 0;
        Attacking = false;
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
            if (Random.value < 0.5) animator.SetInteger("moving", 13);
            else animator.SetInteger("moving", 14);
            base.Die();
        }
    }

    private void CheckDeath()
    {
        if (Dying && (animator.GetInteger("moving") != 13 && animator.GetInteger("moving") != 12))
        {
            if (Random.value < 0.5) animator.SetInteger("moving", 13);
            else animator.SetInteger("moving", 14);
        }
    }
}
