public class Item_BasicBullets : AbstractItem
{
    public override void ApplyStats()
    {
        PlayerStats.Instance.rangeDamageMultiplier *= (float) 1.2; // 20% increase
    }
}