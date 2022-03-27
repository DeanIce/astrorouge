public class Item_HeartbeatMonitor : AbstractItem
{
    public override void ApplyStats()
    {
        PlayerStats.Instance.healthRegen *= (float) 1.05; // 5% increase
    }
}