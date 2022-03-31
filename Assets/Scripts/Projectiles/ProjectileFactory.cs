using UnityEngine;
using UnityEngine.Animations;

public class ProjectileFactory : MonoBehaviour
{
    // All projectile prefabs
    [SerializeField] private GameObject basicProjectile;
    [SerializeField] private GameObject beamProjectile;
    [SerializeField] private GameObject gravityProjectile;
    [SerializeField] private GameObject hitscanProjectile;
    [SerializeField] private GameObject instantaneousBoxProjectile;
    public static ProjectileFactory Instance { get; private set; }

    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    public GameObject CreateBasicProjectile(Vector3 position, Vector3 velocity, LayerMask collidesWith, float lifeSpan,
        float damage, float health = 1)
    {
        GameObject newProjectile = Instantiate(basicProjectile);
        newProjectile.transform.parent = transform;

        newProjectile.transform.position = position;

        newProjectile.GetComponent<BasicProjectile>()
            .InitializeValues(velocity, collidesWith, lifeSpan, health, damage);

        newProjectile.transform.rotation = Quaternion.LookRotation(velocity, newProjectile.transform.up);

        return newProjectile;
    }

    public GameObject CreateBeamProjectile(Vector3 position, Vector3 direction, LayerMask collidesWith,
        LayerMask stopsAt, float duration, float tickTime, float damage, float range)
    {
        GameObject newProjectile = Instantiate(beamProjectile);
        newProjectile.transform.parent = transform;
        newProjectile.GetComponent<BeamProjectile>().InitializeValues(collidesWith, duration, tickTime, damage);
        newProjectile.transform.SetPositionAndRotation(position,
            newProjectile.transform.rotation * Quaternion.FromToRotation(newProjectile.transform.forward, direction));

        newProjectile.GetComponent<BeamProjectile>().ExtendBeam(stopsAt, range);

        return newProjectile;
    }

    public GameObject CreateHitscanProjectile(Vector3 position, Vector3 direction, LayerMask collidesWith, float damage,
        float range)
    {
        GameObject newProjectile = Instantiate(hitscanProjectile);
        newProjectile.transform.parent = transform;
        newProjectile.GetComponent<HitscanProjectile>().InitializeValues(collidesWith, damage, range);
        newProjectile.transform.SetPositionAndRotation(position,
            newProjectile.transform.rotation * Quaternion.FromToRotation(newProjectile.transform.forward, direction));

        return newProjectile;
    }

    public GameObject CreateGravityProjectile(Vector3 position, Vector3 velocity, LayerMask collidesWith,
        float lifeSpan, float damage, float health = 1)
    {
        GameObject newProjectile = Instantiate(gravityProjectile);
        newProjectile.transform.parent = transform;
        newProjectile.GetComponent<GravityProjectile>()
            .InitializeValues(velocity, collidesWith, lifeSpan, health, damage);
        newProjectile.transform.position = position;

        return newProjectile;
    }

    public GameObject CreateInstantaneousProjectile(Vector3 position, Quaternion orientation, float depth, LayerMask collidesWith,
        float damage)
    {
        GameObject newProjectile = Instantiate(instantaneousBoxProjectile);
        newProjectile.transform.parent = transform;
        newProjectile.GetComponent<InstantaneousProjectile>()
            .InitializeValues(collidesWith, damage);
        newProjectile.transform.SetPositionAndRotation(position, orientation);
        newProjectile.transform.localScale = new(1, 1, depth);

        return newProjectile;
    }

    public void AddBurn(GameObject projectile)
    {
        projectile.GetComponent<IProjectile>().AttachEffect(new BurnEffect());
        if (projectile.GetComponent<BasicProjectile>() != null)
        {
            projectile.GetComponent<BasicProjectile>().ActivateBurnTrail(true);
        }
    }
    public void AddPoison(GameObject projectile)
    {
        projectile.GetComponent<IProjectile>().AttachEffect(new PoisonEffect());
        if (projectile.GetComponent<BasicProjectile>() != null)
        {
            projectile.GetComponent<BasicProjectile>().ActivatePoisonTrail(true);
        }
    }
    public void AddLightning(GameObject projectile)
    {
        projectile.GetComponent<IProjectile>().AttachEffect(new LightningEffect());
        if (projectile.GetComponent<BasicProjectile>() != null)
        {
            projectile.GetComponent<BasicProjectile>().ActivateLightningTrail(true);
        }
    }
    public void AddRadioactive(GameObject projectile)
    {
        projectile.GetComponent<IProjectile>().AttachEffect(new RadioactiveEffect());
        if (projectile.GetComponent<BasicProjectile>() != null)
        {
            projectile.GetComponent<BasicProjectile>().ActivateRadioactiveTrail(true);
        }
    }
    public void AddSmite(GameObject projectile)
    {
        projectile.GetComponent<IProjectile>().AttachEffect(new SmiteEffect());
        if (projectile.GetComponent<BasicProjectile>() != null)
        {
            projectile.GetComponent<BasicProjectile>().ActivateSmiteTrail(true);
        }
    }
    public void AddSlow(GameObject projectile)
    {
        projectile.GetComponent<IProjectile>().AttachEffect(new SlowEffect());
        if (projectile.GetComponent<BasicProjectile>() != null)
        {
            projectile.GetComponent<BasicProjectile>().ActivateSlowTrail(true);
        }
    }
    public void AddStun(GameObject projectile)
    {
        projectile.GetComponent<IProjectile>().AttachEffect(new StunEffect());
        if (projectile.GetComponent<BasicProjectile>() != null)
        {
            projectile.GetComponent<BasicProjectile>().ActivateStunTrail(true);
        }
    }
    public void AddMartyrdom(GameObject projectile)
    {
        projectile.GetComponent<IProjectile>().AttachEffect(new MartyrdomEffect());
        if (projectile.GetComponent<BasicProjectile>() != null)
        {
            projectile.GetComponent<BasicProjectile>().ActivateMatyrdomTrail(true);
        }
    }
    public void AddIgnite(GameObject projectile)
    {
        projectile.GetComponent<IProjectile>().AttachEffect(new IgniteEffect());
        if (projectile.GetComponent<BasicProjectile>() != null)
        {
            projectile.GetComponent<BasicProjectile>().ActivateIgniteTrail(true);
        }
    }
}
