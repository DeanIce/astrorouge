using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gravity;

public class MeleeEnemy : BasicEnemyAgent
{
    //new public variables
    public MeshRenderer rend;
    public float attackRange = 1.5f;

    // Private enemy specific variables
    private bool attacking = false;

    public override void Hunt(Collider target)
    {
        //base.Hunt(target);

        DoGravity();

        Debug.DrawRay(transform.position, Body.transform.forward);
        //attacking
        if (Physics.Raycast(transform.position, Body.transform.forward, attackRange, LayerMask.GetMask("Player"))) 
        //old condition: (Mathf.Abs((TargetRb.transform.position - transform.position).magnitude) < attackRange && !attacking) NEW ELIMINATES NEED FOR GETTER METHOD IN BASE
        {
            //it makes more sense of the !attacking condition to just be above but for some reason it doesn't work there
            if (!attacking) StartCoroutine(Attack());
        }
        else
        {
            if (!attacking) base.Hunt(target);
        }
    }

    private IEnumerator Attack()
    {
        RaycastHit[] hits;
        rend.enabled = true;
        attacking = true;
        yield return new WaitForSeconds(1f);
        hits = Physics.RaycastAll(transform.position, transform.forward, attackRange, LayerMask.GetMask("Player"));
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
        rend.enabled = false;
        attacking = false;
    }
}
