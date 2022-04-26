public class Item_Tomahawk : AbstractItem
{
    public override void ApplyStats()
    {
        PlayerStats.Instance.meleeDamageMultiplier *= (float) 1.05; // 5% increase
    }
}