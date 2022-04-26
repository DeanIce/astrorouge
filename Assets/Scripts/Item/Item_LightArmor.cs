public class Item_LightArmor : AbstractItem
{
    public override void ApplyStats()
    {
        PlayerStats.Instance.armor += 5; // add 5 because this is an int
    }
}