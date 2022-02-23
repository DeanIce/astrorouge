using UnityEngine;

public class RadioactiveEffect : IEffect
{
    public void ApplyEffect(GameObject target)
    {
        StatusEffectManager sem = target.GetComponent<StatusEffectManager>();
        if (sem != null)
            sem.ApplyRadioactive(10);
    }
}
