using System;
using Gravity;
using Planets;
using UnityEditor;
using UnityEngine;
using Utilities;
using Random = System.Random;

namespace Levels
{
    [CreateAssetMenu(fileName = "Level", menuName = "BaseLevel", order = 0)]
    public class LevelScriptableObject : ScriptableObject
    {
        // Todo: boss levels

        public EnemyWeight[] enemies;
        public SpawnObjects.AssetCount[] clusterAssets;
        public SpawnObjects.AssetCount[] environmentAssets;

        [MinMaxSlider(1, 10)] public Vector2Int numPlanets = new(3, 5);
        [MinMaxSlider(1, 50)] public Vector2 scale = new(5, 12);

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
                radii[i] = rngRange(rng, scale.x, scale.y);
                Debug.Log(radii[i]);
            }

            Array.Sort(radii);
            Array.Reverse(radii);

            var points = BallDropper.DropBalls(radii);
            var playerPosition = Vector3.zero;
            for (var i = 0; i < actualNumPlanets; i++)
            {
                // Create planet
                var planet = Instantiate(planetPrefab, points[i], Quaternion.identity);
                planet.transform.parent = root.transform;
                var planetGenerator = planet.GetComponent<PlanetGenerator>();
                planetGenerator.scale = radii[i] - 3f;

                var sphereSource = planet.GetComponent<SphereSource>();
                sphereSource.outerRadius = radii[i];
                sphereSource.outerFalloffRadius = radii[i] + 3f;

                // Generate LOD meshes
                planetGenerator.HandleGameModeGeneration();
                planetGenerator.SetLOD(1);
                //     // Spawn objects
                //     SpawnObjects.SpawnProps(planet, planetGenerator, clusterAssets, environmentAssets, rng);
                //


                // The player should spawn at the lowest planet
                if (points[i] == Vector3.zero) playerPosition = Vector3.right * radii[i];
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

        private Vector3 rngPoint(Random rng)
        {
            return new Vector3((float) rng.NextDouble(), (float) rng.NextDouble(), (float) rng.NextDouble());
        }

        /// <summary>
        ///     Display the level. Must be called after Create()
        /// </summary>
        public void Load(GameObject root, Random rng)
        {
            if (!isCreated) Create(root, rng);

            // Todo: actually change to scene
        }


        [Serializable]
        public class EnemyWeight
        {
            public GameObject prefab;
            public float weight;
        }

        [CustomEditor(typeof(LevelScriptableObject))]
        public class LevelManagerEditor : Editor
        {
            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();

                // EditorGUILayout.MinMaxSlider();
            }
        }
    }
}