using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{ 
    public void TakeDmg(float incDamage, int type = 0, bool isCrit = false);
}
