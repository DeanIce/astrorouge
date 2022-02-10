using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjects : MonoBehaviour
{
    // For procedurally spawning things
    public GameObject planet;
    public GameObject[] environmentAssets;
    public int[] numOfAsset;
    private Planets.PlanetGenerator planetGen;
    private Mesh mesh;
    private Vector3[] vertices;
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
            vertices = planetGen.terrainMeshFilter.sharedMesh.vertices;

            for (int i = 0; i < environmentAssets.Length; i++)
            {
                SpawnObject(vertices, environmentAssets[i], numOfAsset[i]);
            }
            // Flag so we dont run indefinitely
            ran = true;
        }
    }

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
            // FIX PARENT AND SCALE AND ROTATION
            spawnLocation = ObjectSpawnLocation(vertices);
            GameObject placeObject = Instantiate(objectToSpawn, spawnLocation, Quaternion.identity);
            placeObject.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
            placeObject.transform.LookAt(planet.transform.position);
            placeObject.transform.rotation = placeObject.transform.rotation * Quaternion.Euler(-90, 0, 0);

            // Debug
            Debug.Log("Just placed my " + i + "th " + objectToSpawn.name);
            Debug.DrawRay(spawnLocation, transform.TransformDirection(spawnLocation));
        }
    }
}
