public class Item_SpareParts : AbstractItem
{
    public override void ApplyStats()
    {
        PlayerStats.Instance.currentHealth += 30; // quick health med-kit-esque
    }
}