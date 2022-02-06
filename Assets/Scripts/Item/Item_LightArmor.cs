public class Item_LightArmor : AbstractItem
{
    public override void ApplyStats()
    {
        PlayerStats.Instance.armor += 40; // add 40 because this is an int
    }
}