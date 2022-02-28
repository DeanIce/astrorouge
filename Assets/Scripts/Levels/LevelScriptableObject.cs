using System;
using System.Collections.Generic;
using Planets;
using Unity.Jobs;
using UnityEngine;
using Random = System.Random;

namespace Levels
{
    [CreateAssetMenu(fileName = "Level", menuName = "BaseLevel", order = 0)]
    public class LevelScriptableObject : ScriptableObject
    {
        // Todo: boss levels
        public string data;


        public EnemyWeight[] enemies;
        public SpawnObjects.AssetCount[] clusterAssets;
        public SpawnObjects.AssetCount[] environmentAssets;

        [Range(2, 10)] public int numPlanetsMin;

        [Range(2, 10)] public int numPlanetsMax = 10;

        public GameObject planetPrefab;

        private readonly Dictionary<int, JobHandle> jobHandles = new();

        private bool isCreated;


        /// <summary>
        ///     Create the level's world meshes, determine asset placement, etc.
        ///     Expensive process, should be invoked before the level is required.
        ///     Todo: prime target for parallelization
        /// </summary>
        public void Create(GameObject root, Random rng)
        {
            var numPlanets = rng.Next(numPlanetsMin, numPlanetsMax);

            for (var i = 0; i < numPlanets; i++)
            {
                // Create planet
                var position = root.transform.position + Vector3.right * i * 20;
                var planet = Instantiate(planetPrefab, position, Quaternion.identity);
                planet.transform.parent = root.transform;

                // Generate LOD meshes
                var planetGenerator = planet.GetComponent<PlanetGenerator>();
                planetGenerator.HandleGameModeGeneration();
                planetGenerator.SetLOD(1);

                // Spawn objects
                SpawnObjects.SpawnProps(planet, planetGenerator, clusterAssets, environmentAssets);

                // Spawn enemies
                // Todo
            }


            isCreated = true;
        }

        /// <summary>
        ///     Display the level. Must be called after Create()
        /// </summary>
        public void Load(GameObject root, Random rng)
        {
            if (!isCreated)
            {
                Create(root, rng);
            }
        }

        // public struct MyJob : IJobParallelFor
        // {
        //     private readonly NativeContainer<GameObject> root;
        //     private readonly GameObject planetPrefab;
        //
        //     public NativeArray<int> indices;
        //
        //     public MyJob(GameObject gameObject, GameObject o, NativeArray<int> i)
        //     {
        //         root = gameObject;
        //         planetPrefab = o;
        //         indices = i;
        //     }
        //
        //
        //     public void Execute(int index)
        //     {
        //         // Create planet
        //         var position = root.transform.position + Vector3.right * index * 15;
        //         var planet = Instantiate(planetPrefab, position, Quaternion.identity);
        //         planet.transform.parent = root.transform;
        //
        //         // Generate LOD meshes
        //         // Todo
        //
        //         // Spawn objects
        //         // Todo
        //
        //         // Spawn enemies
        //         // Todo
        //     }
        // }

        [Serializable]
        public class EnemyWeight
        {
            public GameObject prefab;
            public float weight;
        }
    }
}