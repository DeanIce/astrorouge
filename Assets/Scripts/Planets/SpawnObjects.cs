using System;
using System.Collections.Generic;
using System.Linq;
using Planets;
using UnityEngine;
using Random = System.Random;

public class SpawnObjects : MonoBehaviour
{
    public static int numPropsSpawned;
    public AssetCount[] clusterAssets;

    public AssetCount[] environmentAssets;
    private Mesh mesh;

    private PlanetGenerator planetGenerator;
    private bool ran;

    private int totalSpawned;
    private List<Vector3> vertices;

    private void Start()
    {
        planetGenerator = GetComponent<PlanetGenerator>();
    }


    public static List<GameObject> SpawnEnemies(Random rng, GameObject planet, AssetCount[] enemies,
        int enemiesOnPlanet, int index)
    {
        var planetGenerator = planet.GetComponent<PlanetGenerator>();
        // Set our vertices guaranteed non null
        List<Vector3> vertices = planetGenerator.spawnObjectVertices.ToList();
        var e = new List<GameObject>();
        foreach (AssetCount assetCount in enemies)
        {
            var count = (int) (assetCount.weightedRatio * enemiesOnPlanet);
            e.AddRange(SpawnEnemy(planet.transform, assetCount.prefab, count, assetCount.scale,
                vertices, planetGenerator.scale, rng, index));
        }

        return e;
    }

    public static void AddProps(Random rng, GameObject planet, AssetCount[] props, int propsOnPlanet)
    {
        var planetGenerator = planet.GetComponent<PlanetGenerator>();
        // Set our vertices guaranteed non null
        List<Vector3> vertices = planetGenerator.spawnObjectVertices.ToList();

        foreach (AssetCount assetCount in props)
        {
            var count = (int) (assetCount.weightedRatio * propsOnPlanet);
            SpawnObject(planet.transform, assetCount.prefab, count, assetCount.scale,
                vertices, planetGenerator.scale, rng);
        }
    }


    public static Vector3 ObjectSpawnLocation(List<Vector3> vertices, float planetScale, Random rng)
    {
        int randIndex = rng.Next(0, vertices.Count);
        Vector3 newLoc = vertices[randIndex] * planetScale;

        // This prevents spawning 2 things at the same location in rare instances
        vertices.RemoveAt(randIndex);

        //Debug.Log("Chose index: " + randIndex + " which is vertex: " + vertices[randIndex]);
        return newLoc;
    }

    public static void SpawnObject(Transform origin, GameObject objectToSpawn, int numToSpawn, Vector3 scale,
        List<Vector3> vertices, float planetScale, Random rng)
    {
        // We need this here so we can set rotation
        for (var i = 0; i < numToSpawn; i++)
        {
            // FIX SCALE
            // Referenced from https://answers.unity.com/questions/974149/creating-objects-which-facing-center-of-a-sphere.html
            Vector3 spawnLocation = ObjectSpawnLocation(vertices, planetScale, rng);
            spawnLocation += origin.position;
            GameObject placeObject = Instantiate(objectToSpawn, spawnLocation, Quaternion.identity);

            // TEMP: Scale down huge assets
            placeObject.transform.localScale = scale;

            // Find the center of our origin
            placeObject.transform.LookAt(origin.position);

            // First orient stemming out from planet
            // THEN randomly rotate on plane, NEED to do in this order
            placeObject.transform.rotation *= Quaternion.Euler(-90, 0, 0);
            placeObject.transform.rotation *= Quaternion.Euler(0, rng.Next(0, 180), 0);

            // Set Parent
            placeObject.transform.parent = origin;
            numPropsSpawned++;

            // Debug
            //Debug.Log("Just placed my " + i + "th " + objectToSpawn.name);
            //Debug.DrawRay(spawnLocation, transform.TransformDirection(spawnLocation));
        }


        // totalSpawned += numToSpawn;
    }

    private static List<GameObject> SpawnEnemy(Transform origin, GameObject objectToSpawn, int numToSpawn,
        Vector3 scale,
        List<Vector3> vertices, float planetScale, Random rng, int index)
    {
        var e = new List<GameObject>();
        // We need this here so we can set rotation
        for (var i = 0; i < numToSpawn; i++)
        {
            // FIX SCALE
            // Referenced from https://answers.unity.com/questions/974149/creating-objects-which-facing-center-of-a-sphere.html
            Vector3 spawnLocation = ObjectSpawnLocation(vertices, planetScale, rng);
            spawnLocation += origin.position;
            GameObject placeObject = Instantiate(objectToSpawn, spawnLocation, Quaternion.identity);
            //placeObject.GetComponent<BasicEnemyAgent>().planet = index;

            // TEMP: Scale down huge assets
            placeObject.transform.localScale = scale;

            // Find the center of our origin
            placeObject.transform.LookAt(origin.position);

            // First orient stemming out from planet
            // THEN randomly rotate on plane, NEED to do in this order
            placeObject.transform.rotation *= Quaternion.Euler(-90, 0, 0);
            placeObject.transform.rotation *= Quaternion.Euler(0, rng.Next(0, 180), 0);

            placeObject.tag = "enemy";

            e.Add(placeObject);

            // Set Parent
            // placeObject.transform.parent = origin;

            // Debug
            //Debug.Log("Just placed my " + i + "th " + objectToSpawn.name);
            //Debug.DrawRay(spawnLocation, transform.TransformDirection(spawnLocation));
        }

        // totalSpawned += numToSpawn;
        return e;
    }

    private static void SpawnCluster(Transform transform, GameObject objectToSpawn, int numToSpawn, Vector3 scale,
        List<Vector3> vertices, Random rng)
    {
        // This is to prevent overflow (i.e. random index is the last element in the vertex list)
        int initialSpawnIndex = rng.Next(vertices.Count - numToSpawn);
        for (var i = 0; i < numToSpawn; i++)
        {
            // We can +i here since we manually prevented overflow in our selection
            // NOTE: Vertices are contiguous in memory
            GameObject placeObject = Instantiate(objectToSpawn, vertices[initialSpawnIndex + i], Quaternion.identity);

            // TEMP: Scale down huge assets
            placeObject.transform.localScale = scale;

            // Find the center of our origin
            placeObject.transform.LookAt(transform.position);

            // First orient stemming out from planet
            // THEN randomly rotate on plane, NEED to do in this order
            placeObject.transform.rotation *= Quaternion.Euler(-90, 0, 0);
            placeObject.transform.rotation *= Quaternion.Euler(0, rng.Next(0, 180), 0);

            // Set Parent
            placeObject.transform.parent = transform;

            // Remove vertex from list so we don't spawn 2 things at same spot
            vertices.RemoveAt(initialSpawnIndex + i);
        }

        // totalSpawned += numToSpawn;
    }

    [Serializable]
    public class AssetCount
    {
        public GameObject prefab;
        [Range(0, 1)] public float ratio;
        public Vector3 scale = Vector3.one;

        [HideInInspector] public float weightedRatio;

        public override string ToString()
        {
            return prefab.name;
        }
    }
}