public class GreenItem : AbstractItem
{
    public override void ApplyStats()
    {
        // (Don't leave unguarded debugs) print("before pick up: " + PlayerStats.Instance.maxHealth);
        var maxHealthIncrease = PlayerStats.Instance.maxHealth * 1.4; // 40% increase
        PlayerStats.Instance.maxHealth = (int) maxHealthIncrease;
        // (Don't leave unguarded debugs) print("after pick up: " + PlayerStats.Instance.maxHealth);
    }
}