using System;
using System.Collections.Generic;
using Levels;
using UnityEditor;
using UnityEngine;

namespace Managers
{
    public class LevelManager : ManagerSingleton<LevelManager>
    {
        public List<LevelDescription> levels = new();

        private int current;

        public LevelDescription CurrentLevel
        {
            get
            {
                if (current >= levels.Count)
                {
                    current = 0;
                }

                return levels[current];
            }
        }


        public void NextLevel()
        {
            if (current < levels.Count - 1)
            {
                current++;
                CurrentLevel.levelScriptableObject.Create();
                LOG($"Loading {CurrentLevel.displayName} level.");
                CurrentLevel.levelScriptableObject.Load();
            }
        }

        public void PrevLevel()
        {
            if (current > 0)
            {
                current--;
                CurrentLevel.levelScriptableObject.Create();
                LOG($"Loading {CurrentLevel.displayName} level.");
                CurrentLevel.levelScriptableObject.Load();
            }
        }

        [Serializable]
        public class LevelDescription
        {
            public string displayName;
            public LevelScriptableObject levelScriptableObject;
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

                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("<"))
                {
                    levelManager.PrevLevel();
                }

                EditorGUILayout.LabelField(
                    $"{currentLevel.displayName} (level {levelManager.current})",
                    centered
                );
                if (GUILayout.Button(">"))
                {
                    levelManager.NextLevel();
                }

                EditorGUILayout.EndHorizontal();

                base.OnInspectorGUI();
            }
        }
    }
}