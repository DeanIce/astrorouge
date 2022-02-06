using UnityEngine;

public interface IProjectile
{
    public Vector3 Displacement(float deltaTime);
    public void TakeDmg(float incDamage);
    public void Die();
}
