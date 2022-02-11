using UnityEngine;

namespace Managers
{
    public class DropManager : MonoBehaviour
    {
        private static GameObject[] staticDrops;

        private static int[] staticWeights;
        /*
     * Over engineering ideas:
     * - Make it so only certain enemies can drop certain objects via editor functionality
     */

        // We can do this with events later
        public GameObject[] drops;
        public int[] weights;

        private void Awake()
        {
            staticDrops = drops;
            staticWeights = weights;
        }

        // This is what enemies will call when they die, all logic done here
        public static void SpawnItem(Vector3 location, Quaternion rotation)
        {
            var spawnItem = GetSpawnItem();
            Debug.Log("Spawn Item " + spawnItem.name + " at " + location + " with rotation " + rotation);
            Instantiate(spawnItem, location, rotation);
        }


        /*
     * Beware all ye who come here, here is my convention
     * Each item is assigned a weight, the higher the number the more likely it is to drop
     * The lower the number, the less likely.
     * All item weights are added together as one big number and then segmented, then a random number is found in said big number
     * Example here:
     * Item 1: Weight 10
     * Item 2: Weight 30
     * Total: 40
     * If random yields 0-10, item 1 will be dropped, 11-30, item 2 will be dropped
     * 
     * IMPORTANT NOTE: WHEN ADDING THINGS TO THE DROP MANAGER IN THE EDITOR, THE INDICES CORRESPOND 1:1
     * THAT IS, INDEX 0 OF DROPS WILL HAVE THE WEIGHT AT INDEX 0 OF WEIGHTS
     */
        private static int GetItemNum()
        {
            var totalWeight = 0;
            foreach (var item in staticWeights) totalWeight += item;
            return Random.Range(0, totalWeight);
        }

        // Will pull from list of ALL available drops, GetItemNum does the logic behind which item is dropped though
        private static GameObject GetSpawnItem()
        {
            // Which item we're going to spawn
            var currentSelection = 0;

            // The sum of weights up to index thus far
            var currentWeightIndex = 0;

            // The weighted number selection
            var selectedWeight = GetItemNum();
            Debug.Log("Item # " + selectedWeight);

            for (var i = 0; i < staticWeights.Length; i++)
            {
                if (selectedWeight > currentWeightIndex)
                    currentSelection = i;
                else
                    // For loop so I don't have to manually play with indices
                    // No reason to go through rest of for loop though if we find our selection
                    break;
                currentWeightIndex += staticWeights[i];
            }

            return staticDrops[currentSelection];
        }
    }
}