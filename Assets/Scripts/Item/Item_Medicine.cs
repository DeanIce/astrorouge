public class Item_Medicine : AbstractItem
{
    public override void ApplyStats()
    {
        // current health = max health
        PlayerStats.Instance.currentHealth = PlayerStats.Instance.maxHealth;
        // max health + 40%
        double maxHealthIncrease = PlayerStats.Instance.maxHealth * 1.4; // 40% increase
        PlayerStats.Instance.maxHealth = (int) maxHealthIncrease; 
        // health regen + 20%
        PlayerStats.Instance.healthRegen *= (float) 1.2; // 20% increase
    }
}