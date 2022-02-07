public class Item_SuperStar : AbstractItem
{
    public override void ApplyStats()
    {
        PlayerStats.Instance.invincibilityDuration *= (float) 1.1; // 10% increase
    }
}