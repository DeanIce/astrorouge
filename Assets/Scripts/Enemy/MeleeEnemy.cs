using System.Collections;
using UnityEngine;

public class MeleeEnemy : BasicEnemyAgent
{
    //[Deprecated]

    //new public variables
    //public MeshRenderer rend;

    // Private enemy specific variables
    private bool attacking;

    // public bool Attacking { get { return attacking; } set { attacking = value; } }

    public override void Hunt(Collider target)
    {
        //base.Hunt(target);

        DoGravity();

        //attacking
        if (Physics.Raycast(transform.position, Body.transform.forward, AttackRange - 0.1f,
                LayerMask.GetMask("Player")))
            //old condition: (Mathf.Abs((TargetRb.transform.position - transform.position).magnitude) < attackRange && !attacking) NEW ELIMINATES NEED FOR GETTER METHOD IN BASE
        {
            //it makes more sense of the !attacking condition to just be above but for some reason it doesn't work there
            if (!attacking && Health > 0) StartCoroutine(Attack());
        }
        else
        {
            if (!attacking) base.Hunt(target);
        }
    }

    public virtual IEnumerator Attack()
    {
        RaycastHit[] hits;
        //rend.enabled = true;
        attacking = true;
        yield return new WaitForSeconds(1f);
        hits = Physics.RaycastAll(transform.position, Body.transform.forward, AttackRange, LayerMask.GetMask("Player"));
        if (hits.Length != 0)
        {
            //check for the player in the things the ray hit by whether it has a PlayerDefault
            foreach (var hit in hits)
            {
                if (hit.collider.gameObject.GetComponent<PlayerDefault>() != null)
                    hit.collider.gameObject.GetComponent<PlayerDefault>().TakeDmg(5);
            }
        }

        //rend.enabled = false;
        attacking = false;
    }
}