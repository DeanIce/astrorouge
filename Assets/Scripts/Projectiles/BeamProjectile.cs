using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class BeamProjectile : BaseProjectile
{
    [SerializeField] private GameObject beamVFX;

    private float tickTime;
    private readonly Dictionary<GameObject, float> hitTimings = new();

    private void Update()
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

        foreach(GameObject obj in hitTimings.Keys.ToList())
        {
            hitTimings[obj] += Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other) => ResolveCollision(other);

    private void OnTriggerStay(Collider other) => ResolveCollision(other);

    private void ResolveCollision(Collider other)
    {
        if (((1 << other.gameObject.layer) | collisionLayer) == collisionLayer)
        {
            GameObject root = other.transform.root.gameObject;
            if (hitTimings.TryGetValue(root, out float timeSinceLastHit))
            {
                if (timeSinceLastHit >= tickTime)
                {
                    hitTimings[root] = 0;
                    CollisionResponse(other.gameObject);
                }
            }
            else
            {
                hitTimings.Add(root, 0);
                CollisionResponse(other.gameObject);
            }
            // Beam does NOT die after delivering damage
        }
    }

    public override Vector3 Displacement(float deltaTime) => Vector3.zero;
    public override void TakeDmg(float incDamage) { }

    public override void Die()
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
    /// <param name="tickTime">How long (in seconds) between damage 'ticks' of the beam</param>
    /// <param name="damage">The damage the beam will do when it collides</param>
    public void InitializeValues(LayerMask collidesWith, float duration, float tickTime, float damage)
    {
        collisionLayer = collidesWith;
        timeLeft = duration;
        this.tickTime = tickTime;
        currHealth = 1;
        this.damage = damage;
    }
}
