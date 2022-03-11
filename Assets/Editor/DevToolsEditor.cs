using Managers;
using UnityEditor;
using UnityEngine;

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
