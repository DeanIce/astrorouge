using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_HoldThis : AbstractItem
{
    public override void ApplyStats()
    {
        PlayerStats.Instance.martyrdomChance += 0.05f;
    }
}
