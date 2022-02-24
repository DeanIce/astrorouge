using Managers;
using UnityEditor;
using UnityEngine;

namespace CustomEditors
{
    public static class DevToolsEditor
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
            }
        }
    }
}