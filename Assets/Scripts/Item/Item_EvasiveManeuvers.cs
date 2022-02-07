public class Item_EvasiveManeuvers : AbstractItem
{
    public override void ApplyStats()
    {
        PlayerStats.Instance.dodgeChance *= (float) 1.1; // 10% increase
    }
}