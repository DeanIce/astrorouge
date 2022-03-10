using UnityEngine;

interface IEnemy : IDamageable
{
    void setSpeed(float speed);
    float getSpeed();
    void Wander(Vector3 direction);
    void Hunt(Collider target);
    void Die();
}
