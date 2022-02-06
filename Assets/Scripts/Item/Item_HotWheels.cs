public class Item_HotWheels : AbstractItem
{
    public override void ApplyStats()
    {
        PlayerStats.Instance.sprintMultiplier *= (float) 1.2; // 20% increase
    }
}