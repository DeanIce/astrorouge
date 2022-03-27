public class Item_SpareParts : AbstractItem
{
    public override void ApplyStats()
    {
        PlayerStats.Instance.maxHealth += 10; // 10 health permanent effect
        PlayerStats.Instance.currentHealth += 30; // potential for overheal
    }
}