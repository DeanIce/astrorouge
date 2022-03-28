public class Item_RifleScope : AbstractItem
{
    public override void ApplyStats()
    {
        PlayerStats.Instance.rangeCritChance *= (float) 1.05; // 5% increase
    }
}