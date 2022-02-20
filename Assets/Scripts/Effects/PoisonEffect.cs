using UnityEngine;

public class PoisonEffect : IEffect
{    
    private int poisonTicks = 4;

    public void ApplyEffect(GameObject target)
    {
        StatusEffectManager sem = target.GetComponent<StatusEffectManager>();
        if (sem != null)
            sem.ApplyPoison(poisonTicks);
    }
}
