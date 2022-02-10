using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnObjects : MonoBehaviour
{
    // For procedurally spawning things
    public GameObject planet;
    private Planets.PlanetGenerator planetGen;
    private Mesh mesh;
    private List<Vector3> vertices;
    bool ran = false;

    // Collection of spawn objects
    public GameObject[] environmentAssets;
    public int[] numOfAsset;
    public GameObject[] clusterAssets;
    public int[] numInCluster;

    private void Start()
    {
        planetGen = GetComponent<Planets.PlanetGenerator>();
    }

    // Fix later
    private void Update()
    {
        // We need to wait for shader stuff to finish, once it does run our gen
        if (planetGen.terrainMeshFilter.sharedMesh.vertices != null && !ran)
        {
            // Set our vertices guaranteed non null
            vertices = planetGen.terrainMeshFilter.sharedMesh.vertices.ToList();

            // Spawn all of our clusters
            for (int j = 0; j < clusterAssets.Length; j++)
            {
                SpawnCluster(clusterAssets[j], numInCluster[j]);
            }

            // Spawn all of our random stuff
            for (int i = 0; i < environmentAssets.Length; i++)
            {
                SpawnObject(environmentAssets[i], numOfAsset[i]);
            }
            // Flag so we dont run indefinitely
            ran = true;
        }
    }

    private Vector3 ObjectSpawnLocation()
    {
        int randIndex = UnityEngine.Random.Range(0, vertices.Count);
        Vector3 newLoc = vertices[randIndex];

        // This prevents spawning 2 things at the same location in rare instances
        vertices.RemoveAt(randIndex);

        //Debug.Log("Chose index: " + randIndex + " which is vertex: " + vertices[randIndex]);
        return newLoc;
    }

    private void SpawnObject(GameObject objectToSpawn, int numToSpawn)
    {
        // We need this here so we can set rotation
        Vector3 spawnLocation = new Vector3();
        for (int i = 0; i < numToSpawn; i++)
        {
            // FIX SCALE
            // Referenced from https://answers.unity.com/questions/974149/creating-objects-which-facing-center-of-a-sphere.html
            spawnLocation = ObjectSpawnLocation();
            GameObject placeObject = Instantiate(objectToSpawn, spawnLocation, Quaternion.identity);

            // TEMP: Scale down huge assets
            placeObject.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);

            // Find the center of our origin
            placeObject.transform.LookAt(planet.transform.position);

            // First orient stemming out from planet
            // THEN randomly rotate on plane, NEED to do in this order
            placeObject.transform.rotation = placeObject.transform.rotation * Quaternion.Euler(-90, 0, 0);
            placeObject.transform.rotation = placeObject.transform.rotation * Quaternion.Euler(0, UnityEngine.Random.Range(0, 180), 0);

            // Set Parent
            placeObject.transform.parent = planet.transform;

            // Debug
            //Debug.Log("Just placed my " + i + "th " + objectToSpawn.name);
            //Debug.DrawRay(spawnLocation, transform.TransformDirection(spawnLocation));
        }
    }

    private void SpawnCluster(GameObject objectToSpawn, int numToSpawn)
    {
        // This is to prevent overflow (i.e. random index is the last element in the vertex list)
        int initialSpawnIndex = UnityEngine.Random.Range(0, vertices.Count - numToSpawn);
        for (int i = 0; i < numToSpawn; i++)
        {
            GameObject placeObject = Instantiate(objectToSpawn, vertices[initialSpawnIndex + i], Quaternion.identity);

            // TEMP: Scale down huge assets
            placeObject.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);

            // Find the center of our origin
            placeObject.transform.LookAt(planet.transform.position);

            // First orient stemming out from planet
            // THEN randomly rotate on plane, NEED to do in this order
            placeObject.transform.rotation = placeObject.transform.rotation * Quaternion.Euler(-90, 0, 0);
            placeObject.transform.rotation = placeObject.transform.rotation * Quaternion.Euler(0, UnityEngine.Random.Range(0, 180), 0);

            // Set Parent
            placeObject.transform.parent = planet.transform;

            // Remove vertex from list so we don't spawn 2 things at same spot
            vertices.RemoveAt(initialSpawnIndex + i);
        }
    }
}
