public class Item_ArmoredBoots : AbstractItem
{
    public override void ApplyStats()
    {
        PlayerStats.Instance.armor += 10; // add 10 because armor is an int
    }
}