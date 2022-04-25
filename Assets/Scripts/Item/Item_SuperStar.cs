public class Item_SuperStar : AbstractItem
{
    public override void ApplyStats()
    {
        PlayerStats.Instance.invincibilityDuration += 25f; // increase by 0.025ms
    }
}