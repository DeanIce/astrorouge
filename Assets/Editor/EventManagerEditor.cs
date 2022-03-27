using Managers;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EventManager))]
public class EventManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var eventManager = (EventManager) target;


        base.OnInspectorGUI();
        EditorGUILayout.LabelField("Trigger Events Manually:", EditorStyles.boldLabel);

        if (GUILayout.Button("Win")) eventManager.Win();

        if (GUILayout.Button("Play")) eventManager.Play();

        if (GUILayout.Button("Pause")) eventManager.Pause();

        if (GUILayout.Button("Menu")) eventManager.Menu();

        if (GUILayout.Button("Recap")) eventManager.Recap();

        if (GUILayout.Button("Enter Boss Room")) eventManager.LoadBoss("TestBossFire");
        if (GUILayout.Button("Leave Boss Room")) eventManager.LoadLevel(1);
    }
}