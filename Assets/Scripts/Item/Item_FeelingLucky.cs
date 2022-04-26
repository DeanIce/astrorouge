using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_FeelingLucky : AbstractItem
{
    public override void ApplyStats()
    {
        for(int i = 0; i < 2; i++)
        {
            GameObject temp = Managers.DropManager.Instance.SpawnItem(transform.position, transform.rotation);
            if (temp.GetComponent<AbstractItem>() != null)
            {
                temp.GetComponent<AbstractItem>().ApplyStats();
            }
            else
            {
                Debug.Log("tried Empty Item: " + temp.gameObject.name);
                i--;
            }
        }
    }
}
