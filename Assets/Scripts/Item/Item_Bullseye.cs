public class Item_Bullseye : AbstractItem
{
    public override void ApplyStats()
    {
        PlayerStats.Instance.rangeProjectileRange *= (float) 1.05; // 5% increase
    }
}