﻿using System;
using System.Diagnostics;
using System.Linq;
using GD.MinMaxSlider;
using Gravity;
using Managers;
using Planets;
using UnityEngine;
using Utilities;
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

        [MinMaxSlider(1, 500)] public Vector2Int totalNumEnemies = new(20, 100);

        [MinMaxSlider(50, 4000)] public Vector2Int totalNumProps = new(200, 400);

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

        private float SphereArea(float r)
        {
            return 4 * Mathf.PI * r * r;
        }

        private (float[], float[]) getRadiiAndAreas(Random rng, int actualNumPlanets)
        {
            var radii = new float[actualNumPlanets];


            for (var i = 0; i < actualNumPlanets; i++)
            {
                radii[i] = rngRange(rng, scale.x, scale.y) + gravityHeight;
            }

            Array.Sort(radii);
            Array.Reverse(radii);

            var areas = new float[actualNumPlanets];
            float areaSum = 0;
            for (var i = 0; i < actualNumPlanets; i++)
            {
                areas[i] = SphereArea(radii[i]);
                areaSum += areas[i];
            }

            var areasRatio = new float[actualNumPlanets];
            for (var i = 0; i < actualNumPlanets; i++)
            {
                areasRatio[i] = areas[i] / areaSum;
            }

            return (radii, areasRatio);
        }

        private void WeightRatio(SpawnObjects.AssetCount[] assets)
        {
            float sum = 0;
            foreach (SpawnObjects.AssetCount assetCount in assets)
            {
                sum += assetCount.ratio;
            }

            foreach (SpawnObjects.AssetCount assetCount in assets)
            {
                assetCount.weightedRatio = assetCount.ratio / sum;
            }
        }


        /// <summary>
        ///     Create the level's world meshes, determine asset placement, etc.
        ///     Expensive process, should be invoked before the level is required.
        ///     Todo: prime target for parallelization
        /// </summary>
        /// <param name="root"></param>
        /// <param name="rng"></param>
        /// <param name="timer"></param>
        /// <returns></returns>
        public Vector3 Create(GameObject root, Random rng, Stopwatch timer)
        {
            PlanetGenerator.spheres.Clear();
            SpawnObjects.numPropsSpawned = 0;

            // Solve range constants
            int actualNumPlanets = rng.Next(numPlanets.x, numPlanets.y);
            int actualNumEnemies = rng.Next(totalNumEnemies.x, totalNumEnemies.y);
            int actualNumProps = rng.Next(totalNumProps.x, totalNumProps.y);
            int bossLevelIndex = rng.Next(0, actualNumPlanets);

            // Determine planet radii and surface areas
            (float[] radii, float[] areaRatios) = getRadiiAndAreas(rng, actualNumPlanets);
            LevelManager.LogTimer(timer, "Solved range constants");

            // Perform simulation
            var ballDropper = GameObject.Find("BallDropper").GetComponent<BallDropper>();
            Vector3[] points = ballDropper.DropBalls(radii, timer);
            LevelManager.LogTimer(timer, "Ball dropper done");

            WeightRatio(enemyAssets);
            WeightRatio(environmentAssets);

            Vector3 playerPosition = Vector3.zero;
            var pgs = new PlanetGenerator[actualNumPlanets];

            // Create each planet
            for (var i = 0; i < actualNumPlanets; i++)
            {
                // Create planet
                GameObject planet = Instantiate(planetPrefab, points[i], Quaternion.identity);
                planet.transform.parent = root.transform;
                pgs[i] = planet.GetComponent<PlanetGenerator>();
                pgs[i].shape.seed = i;
                pgs[i].scale = radii[i] - gravityHeight;

                var sphereSource = planet.GetComponent<SphereSource>();
                sphereSource.outerRadius = radii[i];
                sphereSource.outerFalloffRadius = radii[i] + falloffHeight;

                // Generate LOD meshes
                pgs[i].HandleGameModeGeneration(i);


                // The player should spawn at the lowest planet
                if (points[i] == Vector3.zero) playerPosition = Vector3.right * (radii[i] - gravityHeight + 4f);

                // disable for now
                planet.SetActive(false);
            }


            LevelManager.LogTimer(timer, "Generate planet meshes");


            MeshBaker.BakeAndSetColliders(pgs);

            LevelManager.LogTimer(timer, "Bake mesh colliders");

            for (var i = 0; i < actualNumPlanets; i++)
            {
                GameObject planet = pgs[i].gameObject;
                // Add enemies to the planet
                var enemiesOnPlanet = (int) (actualNumEnemies * areaRatios[i]);
                SpawnObjects.SpawnEnemies(rng, planet, enemyAssets, enemiesOnPlanet);
                // Add props to the planet
                var propsOnPlanet = (int) (actualNumProps * areaRatios[i]);
                SpawnObjects.AddProps(rng, planet, environmentAssets, propsOnPlanet);


                // Spawn the boss level entrance
                if (i == bossLevelIndex) AddBossEntrance(pgs[i], bossLevelEntrance, planet.transform, rng);
            }

            LevelManager.LogTimer(timer, "Spawn items");


            isCreated = true;
            return playerPosition;
        }


        private void AddBossEntrance(PlanetGenerator planetGenerator, GameObject objectToSpawn, Transform origin,
            Random rng)
        {
            Vector3 spawnLocation = SpawnObjects.ObjectSpawnLocation(planetGenerator.spawnObjectVertices.ToList(),
                planetGenerator.scale, rng);
            Vector3 position = origin.position;
            spawnLocation += position;
            GameObject placeObject = Instantiate(objectToSpawn, spawnLocation, Quaternion.identity);


            // Find the center of our origin
            placeObject.transform.LookAt(position);

            // First orient stemming out from planet
            // THEN randomly rotate on plane, NEED to do in this order
            Quaternion rotation = placeObject.transform.rotation;
            rotation *= Quaternion.Euler(-90, 0, 0);
            rotation *= Quaternion.Euler(0, rng.Next(0, 180), 0);
            placeObject.transform.rotation = rotation;

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
            if (!isCreated) Create(root, rng, Stopwatch.StartNew());

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