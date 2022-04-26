public class Item_Jitterbug : AbstractItem
{
    public override void ApplyStats()
    {
        PlayerStats.Instance.movementSpeed *= (float) 1.05; // 5% increase
    }
}