using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_ZeusFavorite : AbstractItem
{
    public override void ApplyStats()
    {
        PlayerStats.Instance.lightningChance += 0.05f;
    }
}
