using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_DivineJudgement : AbstractItem
{
    public override void ApplyStats()
    {
        PlayerStats.Instance.smiteChance += 0.01f;
    }
}
