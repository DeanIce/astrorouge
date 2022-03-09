using UnityEngine;

interface IEnemy : IDamageable
{
    float ChangeSpeed(float scalar);
    void Wander(Vector3 direction);
    void Hunt(Collider target);
    void Die();
}
