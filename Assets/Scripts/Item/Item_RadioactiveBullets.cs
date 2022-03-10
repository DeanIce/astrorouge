using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_RadioactiveBullets : AbstractItem
{
    public override void ApplyStats()
    {
        PlayerStats.Instance.radioactiveChance += 0.05f;
    }
}
