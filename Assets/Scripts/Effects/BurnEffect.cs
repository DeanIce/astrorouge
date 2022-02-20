using UnityEngine;

public class BurnEffect : IEffect
{
    private int burnTicks = 6;

    public void ApplyEffect(GameObject target)
    {
        StatusEffectManager sem = target.GetComponent<StatusEffectManager>();
        if (sem != null)
            sem.ApplyBurn(burnTicks);
    }
}
