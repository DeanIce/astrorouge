using System.Collections.Generic;
using System.Linq;
using Planets;
using UnityEngine;

public class SpawnObjects : MonoBehaviour
{
    // For procedurally spawning things
    public GameObject planet;

    // Collection of spawn objects
    public GameObject[] environmentAssets;
    public int[] numOfAsset;
    public GameObject[] clusterAssets;
    public int[] numInCluster;
    private Mesh mesh;
    private PlanetGenerator planetGen;
    private bool ran;
    private List<Vector3> vertices;

    private void Start()
    {
        planetGen = GetComponent<PlanetGenerator>();
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
            for (var j = 0; j < clusterAssets.Length; j++) SpawnCluster(clusterAssets[j], numInCluster[j]);

            // Spawn all of our random stuff
            for (var i = 0; i < environmentAssets.Length; i++) SpawnObject(environmentAssets[i], numOfAsset[i]);
            // Flag so we dont run indefinitely
            ran = true;
        }
    }

    private Vector3 ObjectSpawnLocation()
    {
        var randIndex = Random.Range(0, vertices.Count);
        var newLoc = vertices[randIndex];

        // This prevents spawning 2 things at the same location in rare instances
        vertices.RemoveAt(randIndex);

        //Debug.Log("Chose index: " + randIndex + " which is vertex: " + vertices[randIndex]);
        return newLoc;
    }

    private void SpawnObject(GameObject objectToSpawn, int numToSpawn)
    {
        // We need this here so we can set rotation
        var spawnLocation = new Vector3();
        for (var i = 0; i < numToSpawn; i++)
        {
            // FIX SCALE
            // Referenced from https://answers.unity.com/questions/974149/creating-objects-which-facing-center-of-a-sphere.html
            spawnLocation = ObjectSpawnLocation();
            spawnLocation += transform.position;
            var placeObject = Instantiate(objectToSpawn, spawnLocation, Quaternion.identity);

            // TEMP: Scale down huge assets
            placeObject.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);

            // Find the center of our origin
            placeObject.transform.LookAt(planet.transform.position);

            // First orient stemming out from planet
            // THEN randomly rotate on plane, NEED to do in this order
            placeObject.transform.rotation = placeObject.transform.rotation * Quaternion.Euler(-90, 0, 0);
            placeObject.transform.rotation =
                placeObject.transform.rotation * Quaternion.Euler(0, Random.Range(0, 180), 0);

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
        var initialSpawnIndex = Random.Range(0, vertices.Count - numToSpawn);
        for (var i = 0; i < numToSpawn; i++)
        {
            // We can +i here since we manually prevented overflow in our selection
            // NOTE: Vertices are contiguous in memory
            var placeObject = Instantiate(objectToSpawn, vertices[initialSpawnIndex + i], Quaternion.identity);

            // TEMP: Scale down huge assets
            placeObject.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);

            // Find the center of our origin
            placeObject.transform.LookAt(planet.transform.position);

            // First orient stemming out from planet
            // THEN randomly rotate on plane, NEED to do in this order
            placeObject.transform.rotation = placeObject.transform.rotation * Quaternion.Euler(-90, 0, 0);
            placeObject.transform.rotation =
                placeObject.transform.rotation * Quaternion.Euler(0, Random.Range(0, 180), 0);

            // Set Parent
            placeObject.transform.parent = planet.transform;

            // Remove vertex from list so we don't spawn 2 things at same spot
            vertices.RemoveAt(initialSpawnIndex + i);
        }
    }
}