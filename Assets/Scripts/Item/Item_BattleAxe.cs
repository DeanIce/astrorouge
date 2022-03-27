public class Item_BattleAxe : AbstractItem
{
    public override void ApplyStats()
    {
        PlayerStats.Instance.meleeCritChance *= (float) 1.05; // 20% increase
    }
}