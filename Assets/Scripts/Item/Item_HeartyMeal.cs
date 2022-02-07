public class Item_HeartyMeal : AbstractItem
{
    public override void ApplyStats()
    {
        // print("before pick up: " + PlayerStats.Instance.maxHealth);
        double maxHealthIncrease = PlayerStats.Instance.maxHealth * 1.4; // 40% increase
        PlayerStats.Instance.maxHealth = (int) maxHealthIncrease; 
        // print("after pick up: " + PlayerStats.Instance.maxHealth);
    }
}
