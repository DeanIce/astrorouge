using UnityEngine;

public interface IProjectile
{
    public void AttachEffect(IEffect effect);
    public Vector3 Displacement(float deltaTime);
    public void TakeDmg(float incDamage);
    public void Die();
}
