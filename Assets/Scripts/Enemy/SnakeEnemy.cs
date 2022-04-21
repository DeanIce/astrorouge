using System.Collections;
using UnityEngine;

public class SnakeEnemy : BasicEnemyAgent
{
    [SerializeField] private GameObject mouth;
    [SerializeField] private float poisonChance = 0.5f;
    [SerializeField] private float projSpeed;
    private Animator animator;
    private ProjectileFactory factory;

    public override void Start()
    {
        health *= (Managers.LevelSelect.Instance.requestedLevel + 1);
        maxHealth = health;
        Dying = false;
        animator = GetComponentInChildren<Animator>();
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
        animator.SetBool("attack3", true);
        yield return new WaitForSeconds(2f);
        if (hits.Length != 0)
        {
            //check for the player in the things the ray hit by whether it has a PlayerDefault
            foreach (var hit in hits)
            {
                if (hit.collider.gameObject.GetComponent<PlayerDefault>() != null)
                {
                    projectile = factory.CreateBasicProjectile(mouth.transform.position,
                        projSpeed * (hit.collider.gameObject.transform.position - mouth.transform.position).normalized,
                        LayerMask.GetMask("Player", "Ground"), 5, 5);
                    factory.SetSkin(projectile, 0);
                    if (Random.value < poisonChance)
                    {
                        factory.AddPoison(projectile);
                    }
                }
            }
        }

        //rend.enabled = false;
        Attacking = false;
        animator.SetBool("attack3", false);
    }

    public override void Die()
    {
        if (!Dying)
        {
            Dying = true;
            animator.SetBool("death", true);
            base.Die();
        }
    }
}