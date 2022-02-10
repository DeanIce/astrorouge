using UnityEngine;

public class ProjectileFactory : MonoBehaviour
{
    public static ProjectileFactory Instance { get; private set; }

    // All projectile prefabs
    [SerializeField] private GameObject basicProjectile;
    [SerializeField] private GameObject beamProjectile;

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

    public void CreateBasicProjectile(Vector3 position, Vector3 velocity, LayerMask collidesWith, float lifeSpan,  float damage, float health = 1)
    {
        GameObject newProjectile = Instantiate(basicProjectile);
        newProjectile.transform.parent = gameObject.transform;
        newProjectile.GetComponent<BasicProjectile>().InitializeValues(velocity, collidesWith, lifeSpan, health, damage);
        newProjectile.transform.position = position;
    }

    public void CreateBeamProjectile(Vector3 position, Vector3 direction, LayerMask collidesWith, LayerMask stopsAt, float duration, float damage, float range)
    {
        GameObject newProjectile = Instantiate(beamProjectile);
        newProjectile.transform.parent = gameObject.transform;
        newProjectile.GetComponent<BeamProjectile>().InitializeValues(collidesWith, duration, damage);
        newProjectile.transform.SetPositionAndRotation(position, 
            newProjectile.transform.rotation * Quaternion.FromToRotation(newProjectile.transform.forward, direction));
        
        newProjectile.GetComponent<BeamProjectile>().ExtendBeam(stopsAt, range);
    }
}
