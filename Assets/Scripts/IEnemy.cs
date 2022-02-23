using UnityEngine;

interface IEnemy : IDamageable
{
    void Wander(Vector3 direction);
    void Hunt(Collider target);
    void Die();
}
