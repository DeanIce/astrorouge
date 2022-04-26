public class Item_Medicine : AbstractItem
{
    public override void ApplyStats()
    {
        double maxHealthIncrease = PlayerStats.Instance.maxHealth * 1.10; // 10% increase
        PlayerStats.Instance.maxHealth = (int) maxHealthIncrease;
        PlayerStats.Instance.currentHealth = PlayerStats.Instance.maxHealth;
        PlayerStats.Instance.healthRegen *= (float) 1.10; // 10% increase
    }
}