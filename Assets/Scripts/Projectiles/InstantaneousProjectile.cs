using UnityEngine;

public class InstantaneousProjectile : BaseProjectile
{
    private bool alive = true;

    private void FixedUpdate()
    {
        // Dies on second frame
        if (!alive) Die();

        alive = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) | collisionLayer) == collisionLayer)
        {
            print($"{other.gameObject.name} hit - enter");
            CollisionResponse(other.gameObject);
        }
    }

    public override Vector3 Displacement(float deltaTime) => Vector3.zero;
    public override void Die()
    {
        Destroy(gameObject);
    }

    /// <summary>
    ///     Sets the initial values for the instantaneous projectile
    /// </summary>
    /// <param name="collidesWith">A layermask of ALL the layers the projectile will collide with</param>
    /// <param name="damage">The damage the projectile will do when it collides</param>
    public void InitializeValues(LayerMask collidesWith, float damage)
    {
        collisionLayer = collidesWith;
        this.damage = damage;
    }
}
