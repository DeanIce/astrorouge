public class Item_JumpingFeather : AbstractItem
{
    public override void ApplyStats()
    {
        PlayerStats.Instance.maxExtraJumps++;
    }
}