using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Levels;
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

        private bool isHackDone = true;


        private Random rng;

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

                StartCoroutine(LoadLevel());
            }
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

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == "LevelScene") isHackDone = false;
        }


        private void LoadBossEvent()
        {
            // UnloadLevel();
        }


        private IEnumerator LoadLevel()
        {
            var timer = Stopwatch.StartNew();

            // rng = new Random(0);
            int seed = DateTime.UtcNow.Millisecond;
            rng = new Random(seed);


            LOGTIMER(timer, "start hard work");

            // Do the hard work
            GameObject root = GetOrCreate();
            Vector3 newPlayerPos = CurrentLevel.Create(root, rng, ballDropper, timer);
            // print($"Creating {id} level.");
            LOGTIMER(timer, "finish Create()");


            // And load in the new level
            // print($"Load {CurrentLevel.displayName}");
            CurrentLevel.Load(root, rng);
            LOGTIMER(timer, "finish Load()");


            // print("Loaded level and snapped Player to spawn point.");
            player.transform.position = newPlayerPos;

            LOGTIMER(timer, "level loading");

            // yield return new WaitForSeconds(.1f);
            yield return default;
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