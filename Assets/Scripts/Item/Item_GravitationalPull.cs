public class Item_GravitationalPull : AbstractItem
{
    public override void ApplyStats()
    {
        PlayerStats.Instance.extraJumpDampaner *= (float) 0.99; // 1% decrease
        PlayerStats.Instance.jumpForce *= (float) 1.01; // 1% increase
    }
}