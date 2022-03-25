using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Cinemachine;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Gravity;
using Levels;
using UnityEngine;
using UnityEngine.Animations;
using Random = System.Random;

namespace Managers
{
    public class LevelManager : MonoBehaviour
    {
        public bool doTransition;
        public List<LevelDescription> levels = new();


        public GameObject player;

        public float transitionDuration = 10f;
        public int current;

        private readonly Stack<string> stack = new();


        private CinemachineDollyCart cinemachineDollyCart;
        private CinemachineSmoothPath cinemachineSmoothPath;

        private Random rng;
        public static LevelManager Instance { get; private set; }

        public LevelDescription CurrentLevel
        {
            get
            {
                if (current >= levels.Count) current = 0;

                return levels[current];
            }
        }

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            if (EventManager.Instance)
                current = EventManager.Instance.requestedScene;
            StartCoroutine(LoadLevel());
        }

        private void Update()
        {
            if (player == null) player = GameObject.Find("PlayerDefault");
        }


        public void LoadLevelSync()
        {
            if (Application.isPlaying)
            {
                StartCoroutine(LoadLevel());
                return;
            }

            string id = CurrentLevel.displayName + stack.Count;
            rng = new Random(CurrentLevel.seed);

            // Do the hard work
            CurrentLevel.root = GetOrCreate(id);
            Vector3 newPlayerPos =
                CurrentLevel.levelScriptableObject.Create(CurrentLevel.root, rng, Stopwatch.StartNew());
            print($"Creating {id} level.");


            // Wait until dolly has moved about halfway


            // Then unload the old level
            if (stack.Count > 0)
            {
                print($"Unload {stack.Peek()}");
                UnloadLevel(stack.Peek());
            }

            // And load in the new level
            print($"Load {CurrentLevel.displayName}");
            CurrentLevel.root = GetOrCreate(id);
            CurrentLevel.levelScriptableObject.Load(CurrentLevel.root, rng);

            stack.Push(id);
        }

        public void StartCoroutineLoadLevel()
        {
            StartCoroutine(LoadLevel());
        }


        public IEnumerator LoadLevel()
        {
            var timer = Stopwatch.StartNew();
            if (cinemachineDollyCart == null)
            {
                cinemachineDollyCart = transform.Find("DollyCart1").gameObject.GetComponent<CinemachineDollyCart>();
                cinemachineSmoothPath = transform.Find("DollyTrack1").gameObject.GetComponent<CinemachineSmoothPath>();
            }

            string id = CurrentLevel.displayName + stack.Count;
            rng = new Random(CurrentLevel.seed);


            if (doTransition)
            {
                // Move out to "deep space"
                Vector3 currentPos = player.transform.position;
                cinemachineSmoothPath.m_Waypoints = new CinemachineSmoothPath.Waypoint[2];
                cinemachineSmoothPath.m_Waypoints[0].position = currentPos;
                cinemachineSmoothPath.m_Waypoints[1].position = currentPos + player.transform.up * 100;
                cinemachineSmoothPath.InvalidateDistanceCache();
                cinemachineDollyCart.m_Position = 0;
                player.GetComponent<PositionConstraint>().constraintActive = true;
                player.GetComponent<PlayerDefault>().useGravity = false;

                Sequence seq = DOTween.Sequence();
                seq.AppendInterval(2f);
                TweenerCore<float, float, FloatOptions> tween = DOTween.To(() => cinemachineDollyCart.m_Position,
                    x => { cinemachineDollyCart.m_Position = x; },
                    1f, transitionDuration);
                tween.SetEase(Ease.InCubic);
                seq.Append(tween);
                seq.AppendInterval(2f);

                // var rotTween = player.transform.DOLocalRotate()


                // Wait until dolly has moved about halfway
                if (Application.isPlaying) yield return seq.WaitForCompletion();
                print("We've reached deep space. Proceed with navigation.");
            }


            // Then unload the old level
            if (stack.Count > 0)
            {
                // print($"Unload {stack.Peek()}");
                UnloadLevel(stack.Peek());
            }

            LogTimer(timer, "start hard work");

            // Do the hard work
            CurrentLevel.root = GetOrCreate(id);
            Vector3 newPlayerPos = CurrentLevel.levelScriptableObject.Create(CurrentLevel.root, rng, timer);
            // print($"Creating {id} level.");
            LogTimer(timer, "finish Create()");


            // And load in the new level
            // print($"Load {CurrentLevel.displayName}");
            CurrentLevel.root = GetOrCreate(id);
            CurrentLevel.levelScriptableObject.Load(CurrentLevel.root, rng);
            LogTimer(timer, "finish Load()");

            stack.Push(id);

            if (doTransition)
            {
                // Move back in to the new solar system
                Vector3 currentPos = player.transform.position;
                Vector3 upAxisAtDestination = GravityManager.GetGravity(newPlayerPos);
                cinemachineSmoothPath.m_Waypoints = new CinemachineSmoothPath.Waypoint[3];
                cinemachineSmoothPath.m_Waypoints[0].position = currentPos;
                cinemachineSmoothPath.m_Waypoints[1].position = newPlayerPos + upAxisAtDestination * -3;
                cinemachineSmoothPath.m_Waypoints[2].position = newPlayerPos;

                cinemachineSmoothPath.InvalidateDistanceCache();
                cinemachineDollyCart.m_Position = 0;
                TweenerCore<float, float, FloatOptions> backTween = DOTween.To(() => cinemachineDollyCart.m_Position,
                    x => { cinemachineDollyCart.m_Position = x; },
                    1f, transitionDuration).SetEase(Ease.InOutCubic);


                // Wait until dolly has moved all the way
                if (Application.isPlaying) yield return backTween.WaitForCompletion();
                player.GetComponent<PositionConstraint>().constraintActive = false;
                player.GetComponent<PlayerDefault>().useGravity = true;
            }
            else
            {
                // print("Loaded level and snapped Player to spawn point.");
                player.transform.position = newPlayerPos;
            }

            LogTimer(timer, "level loading");
        }

        public static void LogTimer(Stopwatch sw, string text)
        {
            print(text + " " + sw.ElapsedMilliseconds + " ms.");
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

        [Serializable]
        public class LevelDescription
        {
            public string displayName;
            public LevelScriptableObject levelScriptableObject;
            public int seed;
            protected internal GameObject root;

            public override string ToString()
            {
                return displayName;
            }
        }
    }
}