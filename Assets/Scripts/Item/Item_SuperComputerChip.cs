public class Item_SuperComputerChip : AbstractItem
{
    public override void ApplyStats()
    {
        PlayerStats.Instance.dashRechargeRate *= (float) -1.05; // 10% decrease
    }
}