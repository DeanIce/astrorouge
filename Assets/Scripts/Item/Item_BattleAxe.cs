public class Item_BattleAxe : AbstractItem
{
    public override void ApplyStats()
    {
        PlayerStats.Instance.meleeCritChance *= (float) 1.2; // 20% increase
    }
}