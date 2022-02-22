using System.Collections.Generic;
using UnityEngine;

public interface IProjectile : IDamageable
{
    public void AttachEffect(IEffect effect);
    public Vector3 Displacement(float deltaTime);
    public void Die();
}
