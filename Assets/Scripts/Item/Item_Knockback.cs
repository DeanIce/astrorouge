public class Item_Knockback : AbstractItem
{
    public override void ApplyStats()
    {
        PlayerStats.Instance.meleeKnockbackForce *= (float) 1.05; // 20% increase
    }
}