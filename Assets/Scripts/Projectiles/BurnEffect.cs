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

    /// <param name="collidesWith">A layermask of ALL the layers the projectile will collide with</param>
    public void InitializeValues(LayerMask collidesWith)
    {
        collisionLayer = collidesWith;
    }
}
