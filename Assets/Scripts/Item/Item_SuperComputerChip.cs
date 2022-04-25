public class Item_SuperComputerChip : AbstractItem
{
    public override void ApplyStats()
    {
        PlayerStats.Instance.dashRechargeRate *= (float) 0.95; // 5% decrease
    }
}