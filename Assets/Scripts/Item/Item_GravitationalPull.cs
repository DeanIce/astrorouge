public class Item_GravitationalPull : AbstractItem
{
    public override void ApplyStats()
    {
        PlayerStats.Instance.extraJumpDampaner *= (float) -1.1; // 10% decrease
        PlayerStats.Instance.jumpForce *= (float) 1.1; // 10% increase
    }
}