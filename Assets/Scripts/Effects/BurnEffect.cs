using UnityEngine;

public class BurnEffect : IEffect
{
    private readonly int burnTicks = 6;

    public void ApplyEffect(GameObject target)
    {
        var sem = target.GetComponent<StatusEffectManager>();
        if (sem != null)
            sem.ApplyBurn(burnTicks, sem.burnDamage);
    }
}