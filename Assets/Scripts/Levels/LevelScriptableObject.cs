using System;
using System.Collections.Generic;
using Planets;
using Unity.Jobs;
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

        private readonly Dictionary<int, JobHandle> jobHandles = new();

        private bool isCreated;

        private void OnEnable()
        {
            // Todo: make this work
            // if (numPlanetsMin > numPlanetsMax)
            // {
            //     numPlanetsMin = numPlanetsMax;
            // }
        }


        /// <summary>
        ///     Create the level's world meshes, determine asset placement, etc.
        ///     Expensive process, should be invoked before the level is required.
        ///     Todo: prime target for parallelization
        /// </summary>
        public void Create(GameObject root, Random rng)
        {
            var numPlanets = rng.Next(this.numPlanets.x, this.numPlanets.y);


            var position = root.transform.position;
            for (var i = 0; i < numPlanets; i++)
            {
                // Create planet

                var planet = Instantiate(planetPrefab, position, Quaternion.identity);
                planet.transform.parent = root.transform;
                var planetGenerator = planet.GetComponent<PlanetGenerator>();

                var sample = rng.NextDouble();
                var scaled = sample * (scale.y - scale.x) + scale.x;
                var f = (float) scaled;
                planetGenerator.scale = f;
                position += f * 3 * Vector3.right;

                // Generate LOD meshes
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