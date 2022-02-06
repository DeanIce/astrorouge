using UnityEngine;

public class ProjectileFactory : MonoBehaviour
{
    public static ProjectileFactory Instance { get; private set; }

    // All projectile prefabs
    [SerializeField] private GameObject basicProjectile;

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
}
