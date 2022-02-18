using Managers;
using UnityEditor;
using UnityEngine;

namespace CustomEditors
{
    public class DevToolsEditor
    {
        [CustomEditor(typeof(DevTools))]
        public class InspectorButton : Editor
        {
            public override void OnInspectorGUI()
            {
                // base.OnInspectorGUI();
                var devTools = (DevTools) target;

                DevTools.drawPlanets = GUILayout.Toggle(DevTools.drawPlanets, "Generate Planets");
                DevTools.logPlanetInfo = GUILayout.Toggle(DevTools.logPlanetInfo, "Log Planet Info");

                //
                // EditorGUILayout.LabelField("Planet Settings:", EditorStyles.boldLabel);
                //
                // var enablePlanets = GUILayout.Toggle(false, "Generate Planets");


                // var eventManager = (EventManager) target;
                // if (GUILayout.Button("Win")) eventManager.Win();
                //
                // if (GUILayout.Button("Play")) eventManager.Play();
                //
                // if (GUILayout.Button("Pause")) eventManager.Pause();
                //
                // if (GUILayout.Button("Menu")) eventManager.Menu();
                //
                // if (GUILayout.Button("Recap")) eventManager.Recap();
            }
        }
    }
}