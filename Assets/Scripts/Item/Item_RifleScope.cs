public class Item_RifleScope : AbstractItem
{
    public override void ApplyStats()
    {
        PlayerStats.Instance.rangeCritChance *= (float) 1.2; // 20% increase
    }
}