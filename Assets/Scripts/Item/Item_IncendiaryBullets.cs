using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_IncendiaryBullets : AbstractItem
{
    public override void ApplyStats()
    {
        PlayerStats.Instance.burnChance += 0.05f;
    }
}
