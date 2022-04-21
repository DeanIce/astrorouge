using UnityEngine;

public class BasicProjectile : BaseProjectile
{
    public GameObject bulletHolePrefab;

    [SerializeField] private GameObject BurnTrail;
    [SerializeField] private GameObject PoisonTrail;
    [SerializeField] private GameObject LightningTrail;
    [SerializeField] private GameObject SmiteTrail;
    [SerializeField] private GameObject StunTrail;
    [SerializeField] private GameObject SlowTrail;
    [SerializeField] private GameObject IgniteTrail;
    [SerializeField] private GameObject MartyrdomTrail;
    [SerializeField] private GameObject RadioActiveTrail;

    // Set at initialization
    private Rigidbody rb;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        rb.MovePosition(transform.position + Displacement(Time.fixedDeltaTime));
    }

    private void Update()
    {
        if (currHealth < 0.01f)
        {
            Die();
            return;
        }

        timeLeft -= Time.deltaTime;
        if (timeLeft < 0) Die();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) | collisionLayer) == collisionLayer)
        {
            CollisionResponse(other.gameObject);
            currHealth = 0;
            // GameObject bulletHole = Instantiate(bulletHolePrefab, other.transform, true);
            // bulletHole.transform.position =
            //     other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
        }
    }


    /// <summary>
    ///     Sets the initial values for the basic projectile
    /// </summary>
    /// <param name="initVelocity">The initial velocity</param>
    /// <param name="lifeSpan">The total life span in seconds</param>
    /// <param name="health">The 'health' of the bullet (for blocking feature)</param>
    /// <param name="collidesWith">A layermask of ALL the layers the projectile will collide with</param>
    /// <param name="damage">The damage the projectile will do when it collides</param>
    public void InitializeValues(Vector3 initVelocity, LayerMask collidesWith, float lifeSpan, float health,
        float damage)
    {
        velocity = initVelocity;
        timeLeft = lifeSpan;
        currHealth = health;
        collisionLayer = collidesWith;
        this.damage = damage;
    }

    public override void Die()
    {
        Destroy(gameObject);
    }

    public void ActivateBurnTrail(bool active)
    {
        BurnTrail.SetActive(active);
    }

    public void ActivatePoisonTrail(bool active)
    {
        PoisonTrail.SetActive(active);
    }

    public void ActivateLightningTrail(bool active)
    {
        LightningTrail.SetActive(active);
    }

    public void ActivateStunTrail(bool active)
    {
        StunTrail.SetActive(active);
    }

    public void ActivateSlowTrail(bool active)
    {
        SlowTrail.SetActive(active);
    }

    public void ActivateSmiteTrail(bool active)
    {
        SmiteTrail.SetActive(active);
    }

    public void ActivateIgniteTrail(bool active)
    {
        IgniteTrail.SetActive(active);
    }

    public void ActivateMatyrdomTrail(bool active)
    {
        MartyrdomTrail.SetActive(active);
    }

    public void ActivateRadioactiveTrail(bool active)
    {
        RadioActiveTrail.SetActive(active);
    }
}