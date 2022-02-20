using UnityEngine;

public class SmiteEffect : IEffect
{
    public void ApplyEffect(GameObject target)
    {
        StatusEffectManager sem = target.GetComponent<StatusEffectManager>();
        if (sem != null)
            sem.ApplySmite();
    }
}
