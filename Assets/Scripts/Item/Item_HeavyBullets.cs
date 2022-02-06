public class Item_HeavyBullets : AbstractItem
{
    public override void ApplyStats()
    {
        PlayerStats.Instance.rangeBaseDamage += 20; // add 20 to the base damage
    }
}