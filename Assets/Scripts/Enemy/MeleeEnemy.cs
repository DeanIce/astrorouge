using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gravity;

public class MeleeEnemy : BasicEnemyAgent
{
    //new public variables
    public MeshRenderer rend;
    public int attackRange = 2;

    //static variable to track how many enemies are currently attacking
    public static int attackers = 0;

    // Private enemy specific variables
    private bool attacking = false;

    public override void Hunt(Collider target)
    {
        DoGravity();
        Rigidbody TargetRb = target.GetComponent<Rigidbody>();

        //attacking
        if (Mathf.Abs((TargetRb.transform.position - transform.position).magnitude) < attackRange && attackers < 2)
        {
            if (!attacking)
            {
                attacking = true;
                attackers++;
                rend.enabled = true;
            }
        }
        else
        {
            if (attacking)
            {
                attacking = false;
                attackers--;
                rend.enabled = false;
            }
            base.Hunt(target);
        }
    }
}
