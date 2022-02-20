using Gravity;
using UnityEngine;

public class GravityProjectile : MonoBehaviour, IProjectile
{
    // Dynamic values
    [SerializeField] private Vector3 velocity;
    [SerializeField] private float timeLeft;
    private float currHealth;

    // Set at initialization
    private Rigidbody rb;
    private LayerMask collisionLayer;
    private float damage;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (currHealth < 0.01f)
        {
            Die();
            return;
        }

        Vector3 sumForce = GravityManager.GetGravity(transform.position, out var upAxis);
        velocity += 0.01f * Time.deltaTime * sumForce;

        rb.MovePosition(transform.position + Displacement(Time.deltaTime));
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0)
        {
            Die();
            return;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) | collisionLayer) == collisionLayer)
        {
            other.gameObject.GetComponent<IEnemy>()?.TakeDmg(damage);
            other.gameObject.GetComponent<IProjectile>()?.TakeDmg(damage);
            other.gameObject.GetComponent<IPlayer>()?.TakeDmg(damage);
            currHealth = 0;
        }
    }

    /// <summary>
    /// Sets the initial values for the basic projectile
    /// </summary>
    /// <param name="initVelocity">The initial velocity</param>
    /// <param name="lifeSpan">The total life span in seconds</param>
    /// <param name="health">The 'health' of the bullet (for blocking feature)</param>
    /// <param name="collidesWith">A layermask of ALL the layers the projectile will collide with</param>
    /// <param name="damage">The damage the projectile will do when it collides</param>
    public void InitializeValues(Vector3 initVelocity, LayerMask collidesWith, float lifeSpan, float health, float damage)
    {
        velocity = initVelocity;
        timeLeft = lifeSpan;
        currHealth = health;
        collisionLayer = collidesWith;
        this.damage = damage;
    }

    public Vector3 Displacement(float deltaTime)
    {
        return deltaTime * velocity;
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
}
