public class Item_Knockback : AbstractItem
{
    public override void ApplyStats()
    {
        PlayerStats.Instance.meleeKnockbackForce *= (float) 1.2; // 20% increase
    }
}