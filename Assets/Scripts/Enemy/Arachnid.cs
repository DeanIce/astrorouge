using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arachnid : BasicEnemyAgent
{
    private Animator animator;
    private ProjectileFactory factory;
    [SerializeField] private GameObject mouth;

    public override void Start()
    {
        health *= (Managers.LevelSelect.Instance.requestedLevel + 1);
        animator = GetComponentInChildren<Animator>();
        animator.SetInteger("battle", 1);
        animator.SetInteger("moving", 2);
        Dying = false;
        factory = ProjectileFactory.Instance;
        base.Start();
    }

    public override IEnumerator Attack()
    {
        RaycastHit[] hits;
        GameObject projectile;
        //rend.enabled = true;
        hits = Physics.RaycastAll(transform.position, Body.transform.forward, AttackRange, LayerMask.GetMask("Player"));
        Attacking = true;
        animator.SetInteger("moving", 14);
        yield return new WaitForSeconds(0.625f);
        if (hits.Length != 0)
        {
            //check for the player in the things the ray hit by whether it has a PlayerDefault
            foreach (var hit in hits)
            {
                if (hit.collider.gameObject.GetComponent<PlayerDefault>() != null)
                {
                    projectile = factory.CreateBasicProjectile(mouth.transform.position,
                        hit.collider.gameObject.transform.position - mouth.transform.position,
                        LayerMask.GetMask("Player", "Ground"), 5, 5);
                }
            }
        }
        animator.SetInteger("moving", 0);
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
        animator.SetInteger("moving", anim);
        yield return new WaitForSeconds(0.05f);
        animator.SetInteger("moving", 0);
    }
}
