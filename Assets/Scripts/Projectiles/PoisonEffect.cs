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

    /// <param name="collidesWith">A layermask of ALL the layers the projectile will collide with</param>
    public void InitializeValues(LayerMask collidesWith)
    {
        collisionLayer = collidesWith;
    }
}
