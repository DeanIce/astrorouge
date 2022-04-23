using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Levels;
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

        public List<GameObject> debug;

        public HudUI hudUI;

        private readonly object valueLock = new();
        private bool done;

        private bool isHackDone = true;

        private string progress;
        private int value;

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

                print("start");
                //ThisGuyRunsWithoutWaiting();
                // ThisGuyWaits();
                // LoadLevel();

                StartCoroutine(LoadLevel());
                // StartCoroutine(DoBigProcess());
            }
            // Todo(CURRENT): loading screen async shit

            hudUI.DoShit(progress);

            // lock (valueLock)
            // {
            //     hudUI.DoShit(value);
            // }


            enemies.RemoveAll(item => item == null);
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

        // public IEnumerator DoBigProcess()
        // {
        //     var timer = Stopwatch.StartNew();
        //     int seed = DateTime.UtcNow.Millisecond;
        //     var rng = new Random(seed);
        //     progress = 10;
        //     yield return null;
        //
        //     // Do the hard work
        //     GameObject root = GetOrCreate();
        //     (Vector3 newPlayerPos, List<List<GameObject>> stuff) = CurrentLevel.Create(root, rng, ballDropper, timer);
        //     enemies = stuff;
        //     debug = enemies[0];
        //     progress = 20;
        //     yield return null;
        //
        //
        //     CurrentLevel.Load(root, rng);
        //     progress = 30;
        //     yield return null;
        //
        //
        //     player.transform.position = newPlayerPos;
        //     progress = 40;
        //     yield return null;
        //
        //
        //     progress = 50;
        //     yield return null;
        //     // etc.
        // }


        private IEnumerator LoadLevel()
        {
            var timer = Stopwatch.StartNew();

            // rng = new Random(0);
            int seed = DateTime.UtcNow.Millisecond;
            var rng = new Random(seed);
            SetProgress("start hard work");
            yield return null;


            // Do the hard work
            GameObject root = GetOrCreate();
            CurrentLevel.Setup(root, rng, ballDropper, timer);
            SetProgress("Setup simulation space.");
            yield return null;

            CurrentLevel.GeneratePlanetMeshes(root, rng, ballDropper, timer);
            SetProgress("Generate planet meshes.");
            yield return null;


            (Vector3 newPlayerPos, List<List<GameObject>> stuff) = CurrentLevel.Create(root, rng, ballDropper, timer);
            enemies = stuff;
            debug = enemies[0];
            SetProgress("Spawn props and enemies.");
            yield return null;


            // And load in the new level
            CurrentLevel.Load(root, rng);
            SetProgress("Load level.");
            yield return null;


            player.transform.position = newPlayerPos;

            SetProgress("level loading");

            done = true;
            yield return default;
        }

        private void SetProgress(string s)
        {
            progress = s;
        }

        internal void RemoveEnemy(int planet, GameObject self)
        {
            enemies[planet].Remove(self);
            if (enemies[planet].Count == 0) EventManager.Instance.PlanetCleared();
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == "LevelScene") isHackDone = false;
        }


        private void LoadBossEvent()
        {
            // UnloadLevel();
        }

        private void ThisGuyRunsWithoutWaiting()
        {
            Task.Run(() =>
            {
                try
                {
                    for (var i = 0; i < 100; i++)
                    {
                        Task.Delay(1000);
                        lock (valueLock)
                        {
                            value = i;
                        }
                        // hudUI.DoShit(i);
                    }
                }
                catch (Exception e)
                {
                    print(e);
                    Console.WriteLine(e);
                    throw;
                }
            });
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