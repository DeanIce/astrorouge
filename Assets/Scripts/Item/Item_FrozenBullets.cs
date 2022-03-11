using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_FrozenBullets : AbstractItem
{
    public override void ApplyStats()
    {
        PlayerStats.Instance.slowChance += 0.05f;
    }
}
