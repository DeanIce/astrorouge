public class Item_HeartbeatMonitor : AbstractItem
{
    public override void ApplyStats()
    {
        PlayerStats.Instance.healthRegen *= (float) 1.20; // 5% increase
    }
}