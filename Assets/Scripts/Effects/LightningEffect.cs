using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningEffect : MonoBehaviour
{
    private LayerMask collisionLayer;

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) | collisionLayer) == collisionLayer && other.GetComponent<StatusEffectManager>() != null)
        {
            other.GetComponent<StatusEffectManager>().ApplyLightning();
        }
    }

    public void InitializeValues(GameObject Projectile)
    {
        collisionLayer = Projectile.GetComponent<BasicProjectile>().collisionLayer;
    }
}
