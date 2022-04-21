public class Item_GravitationalPull : AbstractItem
{
    public override void ApplyStats()
    {
        PlayerStats.Instance.extraJumpDampaner *= (float) 0.98; // 2% decrease
        PlayerStats.Instance.jumpForce *= (float) 1.02; // 2% increase
    }
}