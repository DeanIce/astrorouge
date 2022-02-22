using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeEnemy : RangedEnemy
{
    Animator animator;
    ProjectileFactory factory;
    [SerializeField] GameObject mouth;
    [SerializeField] float poisonChance = 0.5f;

    public override void Start()
    {
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
        Attacking = true;
        animator.SetBool("attack3", true);
        yield return new WaitForSeconds(2f);
        hits = Physics.RaycastAll(transform.position, Body.transform.forward, attackRange, LayerMask.GetMask("Player"));
        if (hits.Length != 0)
        {
            //check for the player in the things the ray hit by whether it has a PlayerDefault
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.gameObject.GetComponent<PlayerDefault>() != null)
                {
                    projectile = factory.CreateBasicProjectile(mouth.transform.position, hit.collider.gameObject.transform.position - mouth.transform.position, LayerMask.GetMask("Player", "Ground"), 5, 5);
                    if (Random.value < poisonChance) factory.AddPoison(projectile);
                }
            }
        }
        //rend.enabled = false;
        Attacking = false;
        animator.SetBool("attack3", false);
    }

    public override void Die()
    {
        if (!Dying) StartCoroutine(DieCo());
    }

    private IEnumerator DieCo()
    {
        Dying = true;
        animator.SetBool("attack3", false);
        animator.SetBool("death", true);
        yield return new WaitForSecondsRealtime(10);
        Destroy(gameObject);
    }
}
