public class Item_HeartyMeal : AbstractItem
{
    public override void ApplyStats()
    {
        double maxHealthIncrease = PlayerStats.Instance.maxHealth * 1.10; // 10% increase
        PlayerStats.Instance.maxHealth = (int) maxHealthIncrease; 
    }
}
