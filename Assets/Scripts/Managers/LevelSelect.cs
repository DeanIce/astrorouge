using System.Collections.Generic;
using System.Diagnostics;
using Levels;
using UnityEngine;
using Random = System.Random;

namespace Managers
{
    public class LevelSelect : ManagerSingleton<LevelSelect>
    {
        [Range(0, 2)] public int requestedLevel;

        public List<LevelScriptableObject> levels = new();

        public GameObject player;
        private readonly Stack<string> stack = new();
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
            LoadLevel();
        }


        public void LoadLevel()
        {
            var timer = Stopwatch.StartNew();

            rng = new Random(0);
            string id = "CurrentLevel.displayName" + stack.Count;


            // Then unload the old level
            if (stack.Count > 0)
            {
                // print($"Unload {stack.Peek()}");
                UnloadLevel(stack.Peek());
            }

            LOGTIMER(timer, "start hard work");

            // Do the hard work
            GameObject root = GetOrCreate(id);
            Vector3 newPlayerPos = CurrentLevel.Create(root, rng, timer);
            // print($"Creating {id} level.");
            LOGTIMER(timer, "finish Create()");


            // And load in the new level
            // print($"Load {CurrentLevel.displayName}");
            root = GetOrCreate(id);
            CurrentLevel.Load(root, rng);
            LOGTIMER(timer, "finish Load()");

            stack.Push(id);


            // print("Loaded level and snapped Player to spawn point.");
            player.transform.position = newPlayerPos;


            LOGTIMER(timer, "level loading");

            // yield return new WaitForSeconds(.1f);
        }

        public void UnloadLevel(string displayName)
        {
            if (displayName == null) return;
            Transform t = transform.Find(displayName);
            if (t != null && displayName.Length > 0) DestroyImmediate(t.gameObject);

            // delete all enemies in the scene
            foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("enemy"))
            {
                Destroy(enemy);
            }
        }

        public void UnloadLevel()
        {
            foreach (string levelName in stack)
            {
                GameObject root = transform.Find(levelName)?.gameObject;

                if (root) DestroyImmediate(root);
            }

            stack.Clear();
        }

        private GameObject GetOrCreate(string gameObjectName)
        {
            // Find/create object
            Transform child = transform.Find(gameObjectName);
            if (!child)
            {
                child = new GameObject(gameObjectName).transform;
                child.parent = transform;
                child.localPosition = Vector3.zero;
                child.localRotation = Quaternion.identity;
                child.localScale = Vector3.one;
                child.gameObject.layer = gameObject.layer;
            }

            return child.gameObject;
        }
    }
}