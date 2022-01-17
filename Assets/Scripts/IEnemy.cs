using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IEnemy
{
    void wander(Vector3 direction);
    void hunt();
    void takeDmg(float dmg);
    void die();
}
