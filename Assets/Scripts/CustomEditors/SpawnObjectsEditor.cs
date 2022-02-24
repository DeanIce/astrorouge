using UnityEditor;

namespace CustomEditors
{
    public static class SpawnObjectsEditor
    {
        [CustomEditor(typeof(SpawnObjects))]
        public class InspectorButton : Editor
        {
            public override void OnInspectorGUI()
            {
                var devTools = (SpawnObjects) target;
                EditorGUILayout.LabelField("Total assets spawned: " + devTools.totalSpawned, EditorStyles.boldLabel);

                base.OnInspectorGUI();
            }
        }
    }
}