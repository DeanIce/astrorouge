using System.Collections;
using UnityEngine;

public class InstantaneousProjectile : BaseProjectile
{
    [SerializeField] private GameObject deathVFX;

    private bool alive = true;
    private bool colliderEnabled;

    private void Start()
    {
        gameObject.GetComponent<Collider>().enabled = true;
        colliderEnabled = true;
    }

    private void FixedUpdate()
    {
        // Dies on second frame
        if (colliderEnabled && !alive)
        {
            gameObject.GetComponent<Collider>().enabled = false;
            colliderEnabled = false;
            if (deathVFX != null)
            {
                // Unneeded code for scaling?
                /*ParticleSystem[] partSystems = deathVFX.GetComponentsInChildren<ParticleSystem>(true);
                foreach (ParticleSystem partSystem in partSystems)
                {
                    var shape = partSystem.GetComponent<ParticleSystem>().shape;
                    shape.radius *= transform.lossyScale.x;
                }*/
                deathVFX.SetActive(true);
            }
        }

        alive = false;
    }

    private void Update()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0)
            Die();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) | collisionLayer) == collisionLayer)
        {
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
    /// <param name="lifeSpan">The time until the projectile is destroyed (doesn't change duration of attack)</param>
    /// <param name="damage">The damage the projectile will do when it collides</param>
    public void InitializeValues(LayerMask collidesWith, float lifeSpan, float damage)
    {
        collisionLayer = collidesWith;
        this.damage = damage;
        timeLeft = lifeSpan;
    }
}
