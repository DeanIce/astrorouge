using UnityEngine;

interface IEnemy
{
    void Wander(Vector3 direction);
    void Hunt(Collider target);
    void TakeDmg(float dmg);
    void Die();
}
