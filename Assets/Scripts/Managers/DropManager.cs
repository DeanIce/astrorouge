using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropManager : MonoBehaviour
{
    // Temporary workaround because I'm dumb
    public GameObject[] drops;
    private static GameObject[] staticDrops;

    private void Awake()
    {
        staticDrops = drops;
    }

    // This is what enemies will call when they die, all logic done here
    public static void SpawnItem(Vector3 location, Quaternion rotation)
    {
        GameObject spawnItem = GetSpawnItem();
        Debug.Log("Spawn Item " + spawnItem.name + " at " + location + " with rotation " + rotation);
        GameObject.Instantiate(spawnItem, location, rotation);
    }

    // Will need more logic to narrow down eligible item nums
    private static int GetItemNum()
    {
        // Example loot table ideas
        // If level one, limit range from 0 to X where X is the beginning of stronger level 2 items
        // If enemy type is X, limit range from Y to Z 
        return Random.Range(0, staticDrops.Length);
    }

    // Will pull from list of ALL available drops, GetItemNum does the logic behind which item is dropped though
    private static GameObject GetSpawnItem()
    {
        int lootNum = GetItemNum();
        return staticDrops[lootNum];
    }
}
