using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_DeathcapsDance : AbstractItem
{
    public override void ApplyStats()
    {
        PlayerStats.Instance.poisonChance += 0.05f;
    }
}
