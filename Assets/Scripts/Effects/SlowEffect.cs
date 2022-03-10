using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowEffect : IEffect
{
    public void ApplyEffect(GameObject target)
    {
        StatusEffectManager sem = target.GetComponent<StatusEffectManager>();
        if (sem != null)
            sem.ApplySlow(5);
    }
}
