using UnityEditor;

namespace CustomEditors
{
    public class SpawnObjectsEditor
    {
        [CustomEditor(typeof(SpawnObjects))]
        public class InspectorButton : Editor
        {
            public override void OnInspectorGUI()
            {
                var devTools = (SpawnObjects) target;
                EditorGUILayout.LabelField("Total assets spawned: " + devTools.totalSpawned, EditorStyles.boldLabel);

                base.OnInspectorGUI();

                // DevTools.drawPlanets = GUILayout.Toggle(DevTools.drawPlanets, "Generate Planets");
                // DevTools.logPlanetInfo = GUILayout.Toggle(DevTools.logPlanetInfo, "Log Planet Info");

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