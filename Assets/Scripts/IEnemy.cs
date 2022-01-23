using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IEnemy
{
    void Wander(Vector3 direction);
    void Hunt();
    void TakeDmg(float dmg);
    void Die();
}
