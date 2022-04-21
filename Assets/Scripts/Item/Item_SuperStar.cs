public class Item_SuperStar : AbstractItem
{
    public override void ApplyStats()
    {
        PlayerStats.Instance.invincibilityDuration *= (float) 1.05; // 5% increase
    }
}