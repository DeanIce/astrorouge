using UnityEngine;

public class BeamProjectile : MonoBehaviour, IProjectile
{
    // Dynamic values
    [SerializeField] private float timeLeft;
    private float currHealth;

    // Set at initialization
    private LayerMask collisionLayer;
    private float damage;

    void FixedUpdate()
    {
        if (currHealth < 0.01f)
        {
            Die();
            return;
        }

        timeLeft -= Time.deltaTime;
        if (timeLeft < 0)
        {
            Die();
            return;
        }
    }

    private void OnTriggerEnter(Collider other) => ResolveCollision(other);

    private void OnTriggerStay(Collider other) => ResolveCollision(other);

    private void ResolveCollision(Collider other)
    {
        if (((1 << other.gameObject.layer) | collisionLayer) == collisionLayer)
        {
            other.gameObject.GetComponent<IEnemy>()?.TakeDmg(damage);
            other.gameObject.GetComponent<IProjectile>()?.TakeDmg(damage);
            other.gameObject.GetComponent<IPlayer>()?.TakeDmg(damage);
            // Beam does NOT die after delivering damage
        }
    }

    public Vector3 Displacement(float deltaTime)
    {
        return Vector3.zero;
    }

    public void TakeDmg(float incDamage)
    {
        currHealth -= incDamage;
        if (currHealth < 0)
            Die();
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    public void ExtendBeam(LayerMask stopsAt, float range)
    {
        bool collisionDetected = Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, range, stopsAt);
        transform.localScale = new Vector3(1,1, collisionDetected ? hit.distance : range);
    }

    /// <summary>
    /// Sets the initial values for the beam projectile
    /// </summary>
    /// <param name="collidesWith">A layermask of ALL the layers the projectile will collide with</param>
    /// <param name="duration">How long (in seconds) the beam will persist</param>
    /// <param name="damage">The damage the beam will do when it collides</param>
    public void InitializeValues(LayerMask collidesWith, float duration, float damage)
    {
        collisionLayer = collidesWith;
        timeLeft = duration;
        currHealth = 1;
        this.damage = damage;
    }
}
