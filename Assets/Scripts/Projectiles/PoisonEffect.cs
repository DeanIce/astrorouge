using UnityEngine;

public class PoisonEffect : MonoBehaviour
{
    // Set at initialization
    private LayerMask collisionLayer;
    private int poisonTicks = 4;

    private void OnTriggerEnter(Collider other)
    {
        //refreshes the amount of poison ticks on an enemy
        if (((1 << other.gameObject.layer) | collisionLayer) == collisionLayer && other.GetComponent<StatusEffectManager>() != null)
        {
            other.GetComponent<StatusEffectManager>().ApplyPoison(poisonTicks);
        }
    }

    public void InitializeValues(GameObject Projectile)
    {
        collisionLayer = Projectile.GetComponent<BasicProjectile>().collisionLayer;
    }
}
