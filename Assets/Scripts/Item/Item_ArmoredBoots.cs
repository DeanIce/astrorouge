public class Item_ArmoredBoots : AbstractItem
{
    public override void ApplyStats()
    {
        PlayerStats.Instance.armor += 2; // add 2 because armor is an int
    }
}