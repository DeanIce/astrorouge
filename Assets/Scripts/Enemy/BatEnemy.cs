using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatEnemy: MeleeEnemy
{
    Animator animator;

    public override void Start()
    {
        animator = GetComponentInChildren<Animator>();
        animator.SetInteger("battle", 1);
        Dying = false;
        base.Start();
    }

    public override IEnumerator Attack()
    {
        RaycastHit[] hits;
        //rend.enabled = true;
        Attacking = true;
        animator.SetInteger("moving", 2);
        yield return new WaitForSeconds(1f);
        hits = Physics.RaycastAll(transform.position, Body.transform.forward, attackRange, LayerMask.GetMask("Player"));
        if (hits.Length != 0)
        {
            //check for the player in the things the ray hit by whether it has a PlayerDefault
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.gameObject.GetComponent<PlayerDefault>() != null)
                {
                    hit.collider.gameObject.GetComponent<PlayerDefault>().TakeDmg(5);
                }
            }
        }
        //rend.enabled = false;
        Attacking = false;
    }

    public override void Die()
    {
        if (!Dying) StartCoroutine(DieCo());
    }

    private IEnumerator DieCo()
    {
        Dying = true;
        animator.SetInteger("moving", 14);
        yield return new WaitForSecondsRealtime(1);
        Destroy(gameObject);
    }
}