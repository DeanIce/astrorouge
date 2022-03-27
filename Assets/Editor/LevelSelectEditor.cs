using System.Linq;
using Levels;
using Managers;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelSelect))]
public class LevelSelectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var levelManager = (LevelSelect) target;
        LevelScriptableObject currentLevel = levelManager.CurrentLevel;
        GUIStyle centered = GUI.skin.label;
        centered.alignment = TextAnchor.MiddleCenter;

        string[] options = levelManager.levels.Select((level, i) => $"{i}: {level.name}").ToArray();

        levelManager.requestedLevel = EditorGUILayout.Popup(levelManager.requestedLevel, options);


        base.OnInspectorGUI();
    }
}