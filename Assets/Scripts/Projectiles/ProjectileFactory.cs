using UnityEngine;

public class ProjectileFactory : MonoBehaviour
{
    public static ProjectileFactory Instance { get; private set; }

    // All projectile prefabs
    [SerializeField] private GameObject basicProjectile;
    [SerializeField] private GameObject beamProjectile;
    [SerializeField] private GameObject gravityProjectile;
    [SerializeField] private GameObject hitscanProjectile;

    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public GameObject CreateBasicProjectile(Vector3 position, Vector3 velocity, LayerMask collidesWith, float lifeSpan,  float damage, float health = 1)
    {
        GameObject newProjectile = Instantiate(basicProjectile);
        newProjectile.transform.parent = gameObject.transform;
        newProjectile.GetComponent<BasicProjectile>().InitializeValues(velocity, collidesWith, lifeSpan, health, damage);
        newProjectile.transform.position = position;

        //testing -> add a method to check and add all determined effects
        //AddPoison(newProjectile);
        //AddBurn(newProjectile);
        //AddLightning(newProjectile);
        //AddSmite(newProjectile);
        //AddRadioactive(newProjectile);

        return newProjectile;
    }

    public GameObject CreateBeamProjectile(Vector3 position, Vector3 direction, LayerMask collidesWith, LayerMask stopsAt, float duration, float damage, float range)
    {
        GameObject newProjectile = Instantiate(beamProjectile);
        newProjectile.transform.parent = gameObject.transform;
        newProjectile.GetComponent<BeamProjectile>().InitializeValues(collidesWith, duration, damage);
        newProjectile.transform.SetPositionAndRotation(position, 
            newProjectile.transform.rotation * Quaternion.FromToRotation(newProjectile.transform.forward, direction));
        
        newProjectile.GetComponent<BeamProjectile>().ExtendBeam(stopsAt, range);

        return newProjectile;
    }

    public GameObject CreateHitscanProjectile(Vector3 position, Vector3 direction, LayerMask collidesWith, float damage, float range)
    {
        GameObject newProjectile = Instantiate(hitscanProjectile);
        newProjectile.transform.parent = gameObject.transform;
        newProjectile.GetComponent<HitscanProjectile>().InitializeValues(collidesWith, damage, range);
        newProjectile.transform.SetPositionAndRotation(position,
            newProjectile.transform.rotation * Quaternion.FromToRotation(newProjectile.transform.forward, direction));

        return newProjectile;
    }

    public GameObject CreateGravityProjectile(Vector3 position, Vector3 velocity, LayerMask collidesWith, float lifeSpan, float damage, float health = 1)
    {
        GameObject newProjectile = Instantiate(gravityProjectile);
        newProjectile.transform.parent = gameObject.transform;
        newProjectile.GetComponent<GravityProjectile>().InitializeValues(velocity, collidesWith, lifeSpan, health, damage);
        newProjectile.transform.position = position;

        return newProjectile;
    }
    
    public void AddBurn(GameObject projectile)
    {
        projectile.GetComponent<IProjectile>().AttachEffect(new BurnEffect());
    }

    public void AddPoison(GameObject projectile)
    {
        projectile.GetComponent<IProjectile>().AttachEffect(new PoisonEffect());
    }

    public void AddLightning(GameObject projectile)
    {
        projectile.GetComponent<IProjectile>().AttachEffect(new LightningEffect());
    }

    public void AddRadioactive(GameObject projectile)
    {
        projectile.GetComponent<IProjectile>().AttachEffect(new RadioactiveEffect());
    }

    public void AddSmite(GameObject projectile)
    {
        projectile.GetComponent<IProjectile>().AttachEffect(new SmiteEffect());
    }
}
