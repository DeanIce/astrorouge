using Managers;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(DevTools))]
    public class DevToolsEditor : UnityEditor.Editor
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