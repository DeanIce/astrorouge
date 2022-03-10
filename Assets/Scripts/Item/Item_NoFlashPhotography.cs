using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_NoFlashPhotography : AbstractItem
{
    public override void ApplyStats()
    {
        PlayerStats.Instance.stunChance += 0.05f;
    }
}
