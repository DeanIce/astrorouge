using UnityEngine;

public class LightningEffect : IEffect
{
    public void ApplyEffect(GameObject target)
    {
        StatusEffectManager sem = target.GetComponent<StatusEffectManager>();
        if (sem != null)
            sem.ApplyLightning();
    }
}
