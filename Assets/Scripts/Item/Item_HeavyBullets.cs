public class Item_HeavyBullets : AbstractItem
{
    public override void ApplyStats()
    {
        PlayerStats.Instance.rangeBaseDamage += 5; // add 5 to the base damage
    }
}