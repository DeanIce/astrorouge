using UnityEditor;
using UnityEngine;

namespace Managers
{
    [ExecuteInEditMode]
    public class DevTools : ManagerSingleton<DevTools>
    {
        public static bool drawPlanets = true;

        public static bool logPlanetInfo = true;
    }


    [CustomEditor(typeof(DevTools))]
    public class DevToolsEditor : Editor
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