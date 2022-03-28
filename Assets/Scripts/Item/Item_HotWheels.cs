public class Item_HotWheels : AbstractItem
{
    public override void ApplyStats()
    {
        PlayerStats.Instance.sprintMultiplier *= (float) 1.05; // 5% increase
    }
}