using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmiteEffect : MonoBehaviour
{
    private LayerMask collisionLayer;

    private void OnTriggerEnter(Collider other)
    {
        //refreshes the amount of burn ticks on an enemy
        if (((1 << other.gameObject.layer) | collisionLayer) == collisionLayer && other.GetComponent<StatusEffectManager>() != null)
        {
            other.GetComponent<StatusEffectManager>().ApplySmite();
        }
    }

    public void InitializeValues(GameObject Projectile)
    {
        collisionLayer = Projectile.GetComponent<BasicProjectile>().collisionLayer;
    }
}
