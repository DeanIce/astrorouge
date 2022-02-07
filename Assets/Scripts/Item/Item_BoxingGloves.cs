public class Item_BoxingGloves : AbstractItem
{
    public override void ApplyStats()
    {
        PlayerStats.Instance.meleeBaseDamage += 20; // add 20 to integer value
    }
}