using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_FeelingLucky : AbstractItem
{
    public override void ApplyStats()
    {
        Managers.DropManager.Instance.SpawnItem(transform.position, transform.rotation).GetComponent<AbstractItem>().ApplyStats();
        Managers.DropManager.Instance.SpawnItem(transform.position, transform.rotation).GetComponent<AbstractItem>().ApplyStats();
        Managers.DropManager.Instance.SpawnItem(transform.position, transform.rotation).GetComponent<AbstractItem>().ApplyStats();
    }
}
