using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MartyrdomEffect : IEffect
{
    public void ApplyEffect(GameObject target)
    {
        StatusEffectManager sem = target.GetComponent<StatusEffectManager>();
        if (sem != null)
            sem.ApplyMartyrdom();
    }
}
