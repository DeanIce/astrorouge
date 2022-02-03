public class Item_BasicBullets : AbstractItem
{
    public override void ApplyStats()
    {
        print("before pick up: " + PlayerStats.Instance.rangeDamageMultiplier);
        PlayerStats.Instance.rangeDamageMultiplier *= (float) 1.2; // 20% increase
        print("after pick up: " + PlayerStats.Instance.rangeDamageMultiplier);
    }
}