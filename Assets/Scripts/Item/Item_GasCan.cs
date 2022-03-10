using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_GasCan : AbstractItem
{
    public override void ApplyStats()
    {
        PlayerStats.Instance.igniteChance += 0.05f;
    }
}
