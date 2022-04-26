using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Grenadier : AbstractItem
{
    public override void ApplyStats()
    {
        PlayerStats.Instance.rangeGrenadeSizeMultiplier += 0.35f;
    }
}
