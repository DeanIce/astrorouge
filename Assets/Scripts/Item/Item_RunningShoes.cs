public class Item_RunningShoes : AbstractItem
{
    public override void ApplyStats()
    {
        PlayerStats.Instance.dashDistance *= (float) 1.05; // 5% increase
    }
}