public class Item_SuperComputerChip : AbstractItem
{
    public override void ApplyStats()
    {
        PlayerStats.Instance.dashRechargeRate *= (float) -1.1; // 10% decrease
    }
}