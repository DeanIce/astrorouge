using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnObjects : MonoBehaviour
{
    // For procedurally spawning things
    public GameObject planet;
    public GameObject[] environmentAssets;
    public int[] numOfAsset;
    private Planets.PlanetGenerator planetGen;
    private Mesh mesh;
    private List<Vector3> vertices;
    bool ran = false;
    public float yOffset;

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

            for (int i = 0; i < environmentAssets.Length; i++)
            {
                SpawnObject(vertices, environmentAssets[i], numOfAsset[i]);
            }
            // Flag so we dont run indefinitely
            ran = true;
        }
    }

    private Vector3 ObjectSpawnLocation(List<Vector3> vertices)
    {
        int randIndex = UnityEngine.Random.Range(0, vertices.Count);
        Vector3 newLoc = vertices[randIndex];

        // This prevents spawning 2 things at the same location in rare instances
        vertices.RemoveAt(randIndex);

        //Debug.Log("Chose index: " + randIndex + " which is vertex: " + vertices[randIndex]);
        return newLoc;
    }

    private void SpawnObject(List<Vector3> vertices, GameObject objectToSpawn, int numToSpawn)
    {
        // We need this here so we can set rotation
        Vector3 spawnLocation = new Vector3();
        for (int i = 0; i < numToSpawn; i++)
        {
            // FIX SCALE
            // Referenced from https://answers.unity.com/questions/974149/creating-objects-which-facing-center-of-a-sphere.html
            spawnLocation = ObjectSpawnLocation(vertices);
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
}
