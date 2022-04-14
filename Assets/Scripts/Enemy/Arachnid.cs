using System.Collections;
using Managers;
using UnityEngine;

public class Arachnid : BasicEnemyAgent
{
    [SerializeField] private float projSpeed;
    [SerializeField] private GameObject mouth;
    private Animator animator;
    private ProjectileFactory factory;

    public override void Start()
    {
        health *= LevelSelect.Instance.requestedLevel + 1;
        maxHealth = health;
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
        yield return WaitForSecondsOrDie(0.625f);
        if (hits.Length != 0 && !Dying)
        {
            //check for the player in the things the ray hit by whether it has a PlayerDefault
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.gameObject.GetComponent<PlayerDefault>() != null)
                {
                    projectile = factory.CreateBasicProjectile(mouth.transform.position,
                        projSpeed * (hit.collider.gameObject.transform.position - mouth.transform.position).normalized,
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
        yield return new WaitForSeconds(0.2f);
        animator.SetInteger("moving", anim);
        yield return new WaitForSeconds(0.1f);
        animator.SetInteger("moving", 0);
    }
}