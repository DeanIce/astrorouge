public class Item_Steroids : AbstractItem
{
    public override void ApplyStats()
    {
        PlayerStats.Instance.meleeAttackDelay *= (float) -1.1; // 10% decrease
    }
}