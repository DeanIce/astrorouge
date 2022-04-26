using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Levels;
using Planets;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = System.Random;

namespace Managers
{
    public class LevelSelect : ManagerSingleton<LevelSelect>
    {
        private const string rootName = "LevelHolder";
        [HideInInspector] public int requestedLevel;

        public List<LevelScriptableObject> levels = new();

        public GameObject player;
        public BallDropper ballDropper;


        public List<List<GameObject>> enemies = new();


        public HudUI hudUI;


        private bool isHackDone = true;

        private string progress;

        public LevelScriptableObject CurrentLevel
        {
            get
            {
                if (requestedLevel >= levels.Count) requestedLevel = 0;

                return levels[requestedLevel];
            }
        }

        private void Start()
        {
            if (SceneManager.GetActiveScene().name == "LevelScene") isHackDone = false;
        }

        private void Update()
        {
            // Hack to avoid callback issues with physics collider trigger
            if (!isHackDone)
            {
                isHackDone = true;
                // We may need to find the player again for some reason
                if (player == null) player = GameObject.Find("PlayerDefault");


                if (hudUI == null) hudUI = GameObject.Find("HUD").GetComponent<HudUI>();
                hudUI.ShowProgressMessage();

                StartCoroutine(LoadLevel());
            }

            if (hudUI != null) hudUI.UpdateProgressMessage(progress);


            if (enemies != null) enemies.RemoveAll(item => item == null);
        }

        private void OnEnable()
        {
            EventManager.Instance.loadBoss += LoadBossEvent;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            EventManager.Instance.loadBoss -= LoadBossEvent;
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }


        private IEnumerator LoadLevel()
        {
            var timer = Stopwatch.StartNew();

            // rng = new Random(0);
            int seed = DateTime.UtcNow.Millisecond;
            var rng = new Random(seed);
            SetProgress("start hard work");
            yield return null;


            // Do the hard work
            SetProgress("Setup simulation space");
            yield return null;
            GameObject root = GetOrCreate();
            CurrentLevel.Setup(rng, timer);

            SetProgress("Simulate planet positions");
            yield return null;
            CurrentLevel.DropBalls(ballDropper, timer);

            SetProgress("Generate planet meshes");
            yield return null;
            CurrentLevel.GeneratePlanetMeshes(root, timer);

            SetProgress("Spawn props");
            yield return null;
            CurrentLevel.SpawnProps(rng, timer);


            LevelScriptableObject.InternalState state = CurrentLevel.state;
            PlanetGenerator[] pgs = state.pgs;


            state.enemiesSpawned = new List<List<GameObject>>();
            for (var i = 0; i < state.actualNumPlanets; i++)
            {
                GameObject planet = pgs[i].gameObject;
                // Add enemies to the planet
                var numEnemiesSpawned = (int) (state.actualNumEnemies * state.areaRatios[i]);
                state.enemiesSpawned.Add(SpawnObjects.SpawnEnemies(rng, planet, CurrentLevel.enemyAssets,
                    numEnemiesSpawned, i));

                Instance.LOGTIMER(timer, "Spawn enemies " + i);

                SetProgress($"Spawn enemies on planet {i + 1}/{pgs.Length}");
                yield return null;
            }


            Vector3 newPlayerPos = CurrentLevel.state.playerPosition;
            enemies = CurrentLevel.state.enemiesSpawned;


            SetProgress("Load level");
            yield return null;
            // And load in the new level
            CurrentLevel.Load(root, rng);


            player.transform.position = newPlayerPos;

            //wait for the level to load a little more
            yield return new WaitForSeconds(1.5f);

            hudUI.HideProgressMessage();
            yield return default;
        }

        public void SetProgress(string s)
        {
            progress = s;
        }

        internal void RemoveEnemy(int planet, GameObject self)
        {
            if (enemies != null)
            {
                enemies[planet].Remove(self);
                if (enemies[planet].Count == 0) EventManager.Instance.PlanetCleared();
            }
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == "LevelScene") isHackDone = false;
        }


        private void LoadBossEvent()
        {
            // UnloadLevel();
        }


        public void UnloadLevel()
        {
            GameObject root = transform.Find(rootName)?.gameObject;

            if (root) Destroy(root);
        }

        private GameObject GetOrCreate()
        {
            // Find/create object
            Transform child = new GameObject(rootName).transform;
            child.localPosition = Vector3.zero;
            child.localRotation = Quaternion.identity;
            child.localScale = Vector3.one;
            GameObject o = child.gameObject;
            o.layer = gameObject.layer;

            return o;
        }
    }
}