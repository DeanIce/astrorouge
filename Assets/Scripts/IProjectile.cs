using UnityEngine;

public interface IProjectile : IDamageable
{
    public void AttachEffect(IEffect effect);

    /// <summary>
    /// Copies effects tied to current projectile to target projectile
    /// </summary>
    /// <param name="target">Projectile to gain copies of effects</param>
    public void CopyEffects(IProjectile target);
    public Vector3 Displacement(float deltaTime);
    public void Die();
}
