using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropManager : MonoBehaviour
{
    // Vars
    GameObject itemToSpawn;

    public static void SpawnItem(Vector3 location)
    {
        int lootNum = GetItemNum();
        Debug.Log("Spawn Item " + lootNum + " at " + location);
    }
    
    private static int GetItemNum()
    {
        return 1;
    }
}
