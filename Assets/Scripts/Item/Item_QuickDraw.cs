public class Item_QuickDraw : AbstractItem
{
    public override void ApplyStats()
    {
        PlayerStats.Instance.rangeAttackDelay -= (float) -0.005;
    }
}