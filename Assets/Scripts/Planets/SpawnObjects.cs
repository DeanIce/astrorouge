using System;
using System.Collections.Generic;
using System.Linq;
using Planets;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnObjects : MonoBehaviour
{
    public AssetCount[] clusterAssets;

    public AssetCount[] environmentAssets;
    private Mesh mesh;
    private PlanetGenerator planetGen;

    private PlanetGenerator planetGenerator;
    private bool ran;

    protected internal int totalSpawned;
    private List<Vector3> vertices;

    private void Start()
    {
        planetGen = GetComponent<PlanetGenerator>();
        planetGenerator = GetComponent<PlanetGenerator>();
    }

    // Fix later
    private void Update()
    {
        // We need to wait for shader stuff to finish, once it does run our gen
        if (planetGen.terrainMeshFilter && planetGen.terrainMeshFilter.sharedMesh.vertices != null && !ran)
        {
            // Set our vertices guaranteed non null
            vertices = planetGen.terrainMeshFilter.sharedMesh.vertices.ToList();

            // Spawn all of our clusters
            foreach (var pair in clusterAssets)
            {
                var asset = pair.prefab;
                var count = pair.count;
                var scale = pair.scale;
                SpawnCluster(asset, count, scale);
            }

            // Spawn all of our random stuff
            foreach (var pair in environmentAssets)
            {
                var asset = pair.prefab;
                var count = pair.count;
                var scale = pair.scale;
                SpawnObject(asset, count, scale);
            }

            // for (var i = 0; i < environmentAssets.Length; i++) SpawnObject(environmentAssets[i], numOfAsset[i]);
            // Flag so we dont run indefinitely
            ran = true;
        }
    }

    private Vector3 ObjectSpawnLocation()
    {
        var randIndex = Random.Range(0, vertices.Count);
        var newLoc = vertices[randIndex] * planetGenerator.scale;

        // This prevents spawning 2 things at the same location in rare instances
        vertices.RemoveAt(randIndex);

        //Debug.Log("Chose index: " + randIndex + " which is vertex: " + vertices[randIndex]);
        return newLoc;
    }

    private void SpawnObject(GameObject objectToSpawn, int numToSpawn, Vector3 scale)
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
            placeObject.transform.localScale = scale;

            // Find the center of our origin
            placeObject.transform.LookAt(transform.position);

            // First orient stemming out from planet
            // THEN randomly rotate on plane, NEED to do in this order
            placeObject.transform.rotation *= Quaternion.Euler(-90, 0, 0);
            placeObject.transform.rotation *= Quaternion.Euler(0, Random.Range(0, 180), 0);

            // Set Parent
            placeObject.transform.parent = transform;

            // Debug
            //Debug.Log("Just placed my " + i + "th " + objectToSpawn.name);
            //Debug.DrawRay(spawnLocation, transform.TransformDirection(spawnLocation));
        }

        totalSpawned += numToSpawn;
    }

    private void SpawnCluster(GameObject objectToSpawn, int numToSpawn, Vector3 scale)
    {
        // This is to prevent overflow (i.e. random index is the last element in the vertex list)
        var initialSpawnIndex = Random.Range(0, vertices.Count - numToSpawn);
        for (var i = 0; i < numToSpawn; i++)
        {
            // We can +i here since we manually prevented overflow in our selection
            // NOTE: Vertices are contiguous in memory
            var placeObject = Instantiate(objectToSpawn, vertices[initialSpawnIndex + i], Quaternion.identity);

            // TEMP: Scale down huge assets
            placeObject.transform.localScale = scale;

            // Find the center of our origin
            placeObject.transform.LookAt(transform.position);

            // First orient stemming out from planet
            // THEN randomly rotate on plane, NEED to do in this order
            placeObject.transform.rotation *= Quaternion.Euler(-90, 0, 0);
            placeObject.transform.rotation *= Quaternion.Euler(0, Random.Range(0, 180), 0);

            // Set Parent
            placeObject.transform.parent = transform;

            // Remove vertex from list so we don't spawn 2 things at same spot
            vertices.RemoveAt(initialSpawnIndex + i);
        }

        totalSpawned += numToSpawn;
    }
    // Collection of spawn objects
    // public GameObject[] environmentAssets;
    // public int[] numOfAsset;

    [Serializable]
    public class AssetCount
    {
        public GameObject prefab;
        public int count;
        public Vector3 scale = Vector3.one;
    }
}