public class Item_LightArmor : AbstractItem
{
    public override void ApplyStats()
    {
        PlayerStats.Instance.armor += 25; // add 25 because this is an int
    }
}