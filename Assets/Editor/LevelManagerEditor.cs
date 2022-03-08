using System.Linq;
using Managers;
using UnityEditor;
using UnityEngine;

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


        if (GUILayout.Button("Load Level")) levelManager.LoadLevelSync();

        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Unload All")) levelManager.UnloadLevel();

        base.OnInspectorGUI();
    }
}
