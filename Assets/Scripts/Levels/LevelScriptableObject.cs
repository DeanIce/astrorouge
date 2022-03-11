using System;
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

        public float gravityHeight;
        public float falloffHeight;

        public GameObject planetPrefab;


        private bool isCreated;


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
            var actualNumPlanets = rng.Next(numPlanets.x, numPlanets.y);

            var radii = new float[actualNumPlanets];


            for (var i = 0; i < actualNumPlanets; i++)
            {
                radii[i] = rngRange(rng, scale.x, scale.y) + gravityHeight;
            }

            Array.Sort(radii);
            Array.Reverse(radii);

            var ballDropper = GameObject.Find("BallDropper").GetComponent<BallDropper>();

            var points = ballDropper.DropBalls(radii);
            var playerPosition = Vector3.zero;
            for (var i = 0; i < actualNumPlanets; i++)
            {
                // Create planet
                var planet = Instantiate(planetPrefab, points[i], Quaternion.identity);
                planet.transform.parent = root.transform;
                var planetGenerator = planet.GetComponent<PlanetGenerator>();
                planetGenerator.scale = radii[i] - gravityHeight;

                var sphereSource = planet.GetComponent<SphereSource>();
                sphereSource.outerRadius = radii[i];
                sphereSource.outerFalloffRadius = radii[i] + falloffHeight;

                // Generate LOD meshes
                planetGenerator.HandleGameModeGeneration();
                planetGenerator.SetLOD(1);

                // Spawn objects
                SpawnObjects.SpawnProps(
                    planet,
                    planetGenerator,
                    clusterAssets,
                    environmentAssets,
                    enemyAssets,
                    rng
                );


                // The player should spawn at the lowest planet
                if (points[i] == Vector3.zero) playerPosition = Vector3.right * (radii[i] - gravityHeight + 4f);

                // disable for now
                planet.SetActive(false);
            }


            isCreated = true;
            return playerPosition;
        }

        private float rngRange(Random rng, float start, float end)
        {
            var sample = rng.NextDouble();
            var scaled = sample * (end - start) + start;
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
                var child = root.transform.GetChild(i);
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