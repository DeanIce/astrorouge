using System;
using UnityEngine;

namespace Levels
{
    [CreateAssetMenu(fileName = "Level", menuName = "BaseLevel", order = 0)]
    public class LevelScriptableObject : ScriptableObject
    {
        // Todo: boss levels
        public string data;


        public EnemyWeight[] enemies;
        public SpawnObjects.AssetCount[] clusterAssets;
        public SpawnObjects.AssetCount[] environmentAssets;

        [Range(0, 10)] public int numOfPlanetsMin;

        [Range(0, 10)] public int numOfPlanetsMax = 10;

        public GameObject planetPrefab;

        private bool isCreated;

        /// <summary>
        ///     Create the level's world meshes, determine asset placement, etc.
        ///     Expensive process, should be invoked before the level is required.
        /// </summary>
        public void Create()
        {
            // Do a bunch of expensive calculations

            isCreated = true;
        }

        /// <summary>
        ///     Display the level. Must be called after Create()
        /// </summary>
        public void Load()
        {
            if (!isCreated)
            {
                Create();
            }
        }

        [Serializable]
        public class EnemyWeight
        {
            public GameObject prefab;
            public float weight;
        }
    }
}