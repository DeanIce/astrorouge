public class Item_EvasiveManeuvers : AbstractItem
{
    public override void ApplyStats()
    {
        PlayerStats.Instance.dodgeChance += 0.01f; // 1% increase
    }
}