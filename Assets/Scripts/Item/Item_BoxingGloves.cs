public class Item_BoxingGloves : AbstractItem
{
    public override void ApplyStats()
    {
        PlayerStats.Instance.meleeBaseDamage += 5; // add 5 to integer value
    }
}