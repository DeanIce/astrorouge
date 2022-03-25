public class Item_BasicBullets : AbstractItem
{
    public override void ApplyStats()
    {
        PlayerStats.Instance.rangeDamageMultiplier *= (float) 1.05; // 5% increase
    }
}