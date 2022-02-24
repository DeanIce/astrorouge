public class BlueItem : AbstractItem
{
    public override void ApplyStats()
    {
        // print("before pick up: " + PlayerStats.Instance.maxExtraJumps);
        PlayerStats.Instance.maxExtraJumps++;
        // print("after pick up: " + PlayerStats.Instance.maxExtraJumps);
    }
}