using System;
using System.Collections.Generic;
using System.Linq;
using Levels;
using UnityEditor;
using UnityEngine;
using Random = System.Random;

namespace Managers
{
    public class LevelManager : ManagerSingleton<LevelManager>
    {
        public List<LevelDescription> levels = new();

        public bool transition;

        public GameObject player;
        private int current;

        private Random rng;

        public LevelDescription CurrentLevel
        {
            get
            {
                if (current >= levels.Count) current = 0;

                return levels[current];
            }
        }

        private void Start()
        {
            LoadLevel();
        }


        public void NextLevel()
        {
            if (current < levels.Count - 1) current++;
        }

        public void PrevLevel()
        {
            if (current > 0) current--;
        }

        private void LoadLevel()
        {
            // unload old
            UnloadAll();
            rng = new Random(CurrentLevel.seed);

            // load current
            CurrentLevel.root = GetOrCreate(CurrentLevel.displayName);
            var playerPos = CurrentLevel.levelScriptableObject.Create(CurrentLevel.root, rng);
            player.transform.position = playerPos;
            LOG($"Loading {CurrentLevel.displayName} level.");
            CurrentLevel.levelScriptableObject.Load(CurrentLevel.root, rng);
        }

        private GameObject GetOrCreate(string gameObjectName)
        {
            // Find/create object
            var child = transform.Find(gameObjectName);
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

        private void UnloadAll()
        {
            foreach (var level in levels)
            {
                if (!level.root) level.root = transform.Find(level.displayName)?.gameObject;

                if (level.root) DestroyImmediate(level.root);
            }
        }

        [Serializable]
        public class LevelDescription
        {
            public string displayName;
            public LevelScriptableObject levelScriptableObject;
            public int seed;
            protected internal GameObject root;
        }

        [CustomEditor(typeof(LevelManager))]
        public class LevelManagerEditor : Editor
        {
            public override void OnInspectorGUI()
            {
                var levelManager = (LevelManager) target;
                var currentLevel = levelManager.CurrentLevel;
                var centered = GUI.skin.label;
                centered.alignment = TextAnchor.MiddleCenter;

                var options = levelManager.levels.Select((level, i) => $"{i}: {level.displayName}").ToArray();

                EditorGUILayout.BeginHorizontal();
                levelManager.current = EditorGUILayout.Popup(levelManager.current, options);


                if (GUILayout.Button("Load Level")) levelManager.LoadLevel();

                EditorGUILayout.EndHorizontal();

                if (GUILayout.Button("Unload All")) levelManager.UnloadAll();

                base.OnInspectorGUI();
            }
        }
    }
}