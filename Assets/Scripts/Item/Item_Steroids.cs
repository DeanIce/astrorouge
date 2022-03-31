public class Item_Steroids : AbstractItem
{
    public override void ApplyStats()
    {
        PlayerStats.Instance.meleeAttackDelay -= (float) 0.005;
    }
}