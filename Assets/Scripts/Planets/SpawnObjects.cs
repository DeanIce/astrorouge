using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjects : MonoBehaviour
{
    // For procedurally spawning things
    public GameObject[] environmentAssets;
    public int[] numOfAsset;

    // PROCEDURAL SPAWNING STARTS HERE!
    private Vector3 ObjectSpawnLocation(Vector3[] vertices)
    {
        // BUG: Figure out way to remove vertices from list
        // TODO: Add potential y offset parameter
        int randIndex = UnityEngine.Random.Range(0, vertices.Length);
        Debug.Log("Chose index: " + randIndex + " which is vertex: " + vertices[randIndex]);
        // here
        return vertices[randIndex];
    }

    private void SpawnObject(Vector3[] vertices, GameObject objectToSpawn, int numToSpawn)
    {
        // We need this here so we can set rotation
        Vector3 spawnLocation = new Vector3();
        for (int i = 0; i < numToSpawn; i++)
        {
            spawnLocation = ObjectSpawnLocation(vertices);
            GameObject placeObject = Instantiate(objectToSpawn, spawnLocation, Quaternion.LookRotation(spawnLocation), transform.parent);
            Debug.Log("Just placed my " + i + "th " + objectToSpawn.name);
        }
    }
}
