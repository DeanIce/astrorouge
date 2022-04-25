public class Item_HeavyBullets : AbstractItem
{
    public override void ApplyStats()
    {
        PlayerStats.Instance.rangeBaseDamage += 3; // add 3 to the base damage
    }
}