using UnityEngine;

public class BurnEffect : MonoBehaviour
{
    private LayerMask collisionLayer;
    private int burnTicks = 6;

    private void OnTriggerEnter(Collider other)
    {
        //refreshes the amount of burn ticks on an enemy
        if (((1 << other.gameObject.layer) | collisionLayer) == collisionLayer && other.GetComponent<StatusEffectManager>() != null)
        {
            other.GetComponent<StatusEffectManager>().ApplyBurn(burnTicks);
        }
    }

    public void InitializeValues(GameObject Projectile)
    {
        collisionLayer = Projectile.GetComponent<BasicProjectile>().collisionLayer;
    }
}
