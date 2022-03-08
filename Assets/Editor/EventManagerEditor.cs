using Managers;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(EventManager))]
    public class EventManagerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.LabelField("Trigger Events Manually:", EditorStyles.boldLabel);

            var eventManager = (EventManager) target;
            if (GUILayout.Button("Win")) eventManager.Win();

            if (GUILayout.Button("Play")) eventManager.Play();

            if (GUILayout.Button("Pause")) eventManager.Pause();

            if (GUILayout.Button("Menu")) eventManager.Menu();

            if (GUILayout.Button("Recap")) eventManager.Recap();
        }
    }
}