using System;
using System.Collections.Generic;
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
        public SpawnObjects.AssetCount[] clusterAssets = Array.Empty<SpawnObjects.AssetCount>();
        public SpawnObjects.AssetCount[] environmentAssets = Array.Empty<SpawnObjects.AssetCount>();
        public SpawnObjects.AssetCount[] enemyAssets = Array.Empty<SpawnObjects.AssetCount>();

        [MinMaxSlider(1, 10)] public Vector2Int numPlanets = new(3, 5);
        [MinMaxSlider(1, 50)] public Vector2 scale = new(5, 12);

        [MinMaxSlider(1, 500)] public Vector2Int totalNumEnemies = new(20, 100);

        [MinMaxSlider(50, 10000)] public Vector2Int totalNumProps = new(200, 400);

        public AudioClip levelMusic;

        public float gravityHeight = 3;
        public float falloffHeight = 5;

        public string bossSceneName;
        public GameObject bossLevelEntrance;

        public GameObject planetPrefab;

        public InternalState state;


        private void OnValidate()
        {
            // todo
        }

        private float SphereArea(float r) => 4 * Mathf.PI * r * r;

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

        public void Setup(Random rng, Stopwatch timer)
        {
            state = new InternalState();

            PlanetGenerator.spheres.Clear();
            SpawnObjects.numPropsSpawned = 0;

            // Solve range constants
            state.actualNumPlanets = rng.Next(numPlanets.x, numPlanets.y);
            state.actualNumEnemies = rng.Next(totalNumEnemies.x, totalNumEnemies.y);
            state.actualNumProps = rng.Next(totalNumProps.x, totalNumProps.y);
            state.bossLevelIndex = rng.Next(0, state.actualNumPlanets);

            // Determine planet radii and surface areas
            (float[] radii, float[] areaRatios) = getRadiiAndAreas(rng, state.actualNumPlanets);
            LevelSelect.Instance.LOGTIMER(timer, "Solved range constants");

            state.radii = radii;
            state.areaRatios = areaRatios;
        }

        public void DropBalls(BallDropper ballDropper, Stopwatch timer)
        {
            // Perform simulation
            state.points = ballDropper.DropBalls(state.radii, timer);
            LevelSelect.Instance.LOGTIMER(timer, "Ball dropper done");

            WeightRatio(enemyAssets);
            WeightRatio(environmentAssets);

            state.pgs = new PlanetGenerator[state.actualNumPlanets];
        }

        public void GeneratePlanetMeshes(GameObject root, Stopwatch timer)
        {
            PlanetGenerator[] pgs = state.pgs;
            // Create each planet
            for (var i = 0; i < state.actualNumPlanets; i++)
            {
                // Create planet
                GameObject planet = Instantiate(planetPrefab, state.points[i], Quaternion.identity);
                planet.transform.parent = root.transform;
                pgs[i] = planet.GetComponent<PlanetGenerator>();
                pgs[i].shape.seed = i;
                pgs[i].scale = state.radii[i] - gravityHeight;

                var sphereSource = planet.GetComponent<SphereSource>();
                sphereSource.outerRadius = state.radii[i];
                sphereSource.outerFalloffRadius = state.radii[i] + falloffHeight;

                // Generate LOD meshes
                pgs[i].HandleGameModeGeneration(i);


                // The player should spawn at the lowest planet
                if (state.points[i] == Vector3.zero)
                    state.playerPosition = Vector3.right * (state.radii[i] - gravityHeight + 4f);

                // disable for now
                planet.SetActive(false);
            }

            LevelSelect.Instance.LOGTIMER(timer, "Generate planet meshes");

            // Bake mesh colliders in parallel (Burst compiled)
            MeshBaker.BakeAndSetColliders(pgs);
            LevelSelect.Instance.LOGTIMER(timer, "Bake mesh colliders");
        }


        public void SpawnProps(Random rng, Stopwatch timer)
        {
            PlanetGenerator[] pgs = state.pgs;

            for (var i = 0; i < state.actualNumPlanets; i++)
            {
                GameObject planet = pgs[i].gameObject;
                // Add props to the planet
                var propsOnPlanet = (int) (state.actualNumProps * state.areaRatios[i]);
                SpawnObjects.AddProps(rng, planet, environmentAssets, propsOnPlanet);


                // Spawn the boss level entrance
                if (i == state.bossLevelIndex) AddBossEntrance(pgs[i], bossLevelEntrance, planet.transform, rng);

                StaticBatchingUtility.Combine(planet);
            }

            LevelSelect.Instance.LOGTIMER(timer, "Spawn items");
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
            // if (!isCreated) Create(root, rng, Stopwatch.StartNew());
            if (levelMusic != null) AudioManager.Instance.PlayMusicWithCrossfade(levelMusic);

            for (var i = 0; i < root.transform.childCount; i++)
            {
                Transform child = root.transform.GetChild(i);
                child.gameObject.SetActive(true);
            }
        }

        public struct InternalState
        {
            public PlanetGenerator[] pgs;
            public int actualNumPlanets;
            public int actualNumProps;
            public int actualNumEnemies;
            public int bossLevelIndex;
            public Vector3[] points;
            public float[] radii;
            public float[] areaRatios;
            public Vector3 playerPosition;
            public List<List<GameObject>> enemiesSpawned;
        }


        [Serializable]
        public class EnemyWeight
        {
            public GameObject prefab;
            public float weight;
        }
    }
}