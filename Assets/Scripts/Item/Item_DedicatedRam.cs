public class Item_DedicatedRam : AbstractItem
{
    public override void ApplyStats()
    {
        PlayerStats.Instance.dashCharges++;
    }
}