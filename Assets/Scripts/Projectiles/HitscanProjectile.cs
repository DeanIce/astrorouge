using UnityEngine;

public class HitscanProjectile : MonoBehaviour, IProjectile
{
    // Set at initialization
    private LayerMask collisionLayer;
    private float damage;
    private float range;

    // Update is called once per frame
    void FixedUpdate()
    {
        bool hitDetected = Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, range, collisionLayer);
        if (hitDetected)
        {
            GameObject target = hitInfo.transform.root.gameObject;
            target.GetComponent<IEnemy>()?.TakeDmg(damage);
            target.GetComponent<IProjectile>()?.TakeDmg(damage);
            target.GetComponent<IPlayer>()?.TakeDmg(damage);
        }

        Die();
    }

    /// <summary>
    /// Sets the initial values for the hitscan projectile
    /// </summary>
    /// <param name="collidesWith">A layermask of ALL the layers the projectile will collide with</param>
    /// <param name="damage">The damage the projectile will do when it collides</param>
    /// <param name="range">The distance for the raycast</param>
    public void InitializeValues(LayerMask collidesWith, float damage, float range)
    {
        collisionLayer = collidesWith;
        this.damage = damage;
        this.range = range;
    }

    public Vector3 Displacement(float deltaTime) => Vector3.zero;
    public void TakeDmg(float incDamage) { }

    public void Die()
    {
        Destroy(gameObject);
    }
}
