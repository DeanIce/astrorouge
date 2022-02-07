public class Item_QuickDraw : AbstractItem
{
    public override void ApplyStats()
    {
        PlayerStats.Instance.rangeAttackDelay *= (float) -1.1; // 10% decrease
    }
}