using System;
using System.Linq;
using GD.MinMaxSlider;
using Gravity;
using Planets;
using UnityEngine;
using Random = System.Random;

namespace Levels
{
    [CreateAssetMenu(fileName = "Level", menuName = "BaseLevel", order = 0)]
    public class LevelScriptableObject : ScriptableObject
    {
        // Todo: boss levels

        public SpawnObjects.AssetCount[] clusterAssets = Array.Empty<SpawnObjects.AssetCount>();
        public SpawnObjects.AssetCount[] environmentAssets = Array.Empty<SpawnObjects.AssetCount>();
        public SpawnObjects.AssetCount[] enemyAssets = Array.Empty<SpawnObjects.AssetCount>();

        [MinMaxSlider(1, 10)] public Vector2Int numPlanets = new(3, 5);
        [MinMaxSlider(1, 50)] public Vector2 scale = new(5, 12);

        public float gravityHeight = 3;
        public float falloffHeight = 5;

        public string bossSceneName;
        public GameObject bossLevelEntrance;

        public GameObject planetPrefab;


        private bool isCreated;


        private void OnValidate()
        {
            // todo
        }


        /// <summary>
        ///     Create the level's world meshes, determine asset placement, etc.
        ///     Expensive process, should be invoked before the level is required.
        ///     Todo: prime target for parallelization
        /// </summary>
        /// <param name="root"></param>
        /// <param name="rng"></param>
        /// <returns></returns>
        public Vector3 Create(GameObject root, Random rng)
        {
            int actualNumPlanets = rng.Next(numPlanets.x, numPlanets.y);

            var radii = new float[actualNumPlanets];


            for (var i = 0; i < actualNumPlanets; i++)
            {
                radii[i] = rngRange(rng, scale.x, scale.y) + gravityHeight;
            }

            Array.Sort(radii);
            Array.Reverse(radii);

            var ballDropper = GameObject.Find("BallDropper").GetComponent<BallDropper>();

            Vector3[] points = ballDropper.DropBalls(radii);
            Vector3 playerPosition = Vector3.zero;

            int bossLevelIndex = rng.Next(0, actualNumPlanets);

            for (var i = 0; i < actualNumPlanets; i++)
            {
                // Create planet
                GameObject planet = Instantiate(planetPrefab, points[i], Quaternion.identity);
                planet.transform.parent = root.transform;
                var planetGenerator = planet.GetComponent<PlanetGenerator>();
                planetGenerator.scale = radii[i] - gravityHeight;

                var sphereSource = planet.GetComponent<SphereSource>();
                sphereSource.outerRadius = radii[i];
                sphereSource.outerFalloffRadius = radii[i] + falloffHeight;

                // Generate LOD meshes
                planetGenerator.HandleGameModeGeneration();

                // Spawn objects
                SpawnObjects.SpawnProps(
                    planet,
                    planetGenerator,
                    clusterAssets,
                    environmentAssets,
                    enemyAssets,
                    rng
                );

                // Spawn the boss level entrance
                if (i == bossLevelIndex) AddBossEntrance(planetGenerator, bossLevelEntrance, planet.transform, rng);


                // The player should spawn at the lowest planet
                if (points[i] == Vector3.zero) playerPosition = Vector3.right * (radii[i] - gravityHeight + 4f);

                // disable for now
                planet.SetActive(false);
            }


            isCreated = true;
            return playerPosition;
        }


        private void AddBossEntrance(PlanetGenerator planetGenerator, GameObject objectToSpawn, Transform origin,
            Random rng)
        {
            Vector3 spawnLocation = SpawnObjects.ObjectSpawnLocation(planetGenerator.spawnObjectVertices.ToList(),
                planetGenerator.scale, rng);
            spawnLocation += origin.position;
            GameObject placeObject = Instantiate(objectToSpawn, spawnLocation, Quaternion.identity);


            // Find the center of our origin
            placeObject.transform.LookAt(origin.position);

            // First orient stemming out from planet
            // THEN randomly rotate on plane, NEED to do in this order
            placeObject.transform.rotation *= Quaternion.Euler(-90, 0, 0);
            placeObject.transform.rotation *= Quaternion.Euler(0, rng.Next(0, 180), 0);

            // Set Parent
            placeObject.transform.parent = origin;
            placeObject.GetComponent<BossInstanceEnter>().bossSceneName = bossSceneName;
        }

        private float rngRange(Random rng, float start, float end)
        {
            double sample = rng.NextDouble();
            double scaled = sample * (end - start) + start;
            var f = (float) scaled;
            return f;
        }


        /// <summary>
        ///     Display the level. Must be called after Create()
        /// </summary>
        public void Load(GameObject root, Random rng)
        {
            if (!isCreated) Create(root, rng);

            for (var i = 0; i < root.transform.childCount; i++)
            {
                Transform child = root.transform.GetChild(i);
                child.gameObject.SetActive(true);
            }
        }


        [Serializable]
        public class EnemyWeight
        {
            public GameObject prefab;
            public float weight;
        }
    }
}