using System;
using UnityEngine;

public class SpawnProjectileOnDestroy : MonoBehaviour
{
    public Func<Transform, GameObject> SpawnProjectile { private get; set; }

    private void OnDestroy()
    {
        GameObject inst = SpawnProjectile(transform);
        IProjectile projectile = gameObject.GetComponent<IProjectile>();
        if (projectile != null)
        {
            projectile.CopyEffects(inst.GetComponent<IProjectile>());
        }
    }
}
