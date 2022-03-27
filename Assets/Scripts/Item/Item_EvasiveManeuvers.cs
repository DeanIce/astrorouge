public class Item_EvasiveManeuvers : AbstractItem
{
    public override void ApplyStats()
    {
        PlayerStats.Instance.dodgeChance *= (float) 1.01; // 1% increase
    }
}