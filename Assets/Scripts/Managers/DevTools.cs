using UnityEngine;

namespace Managers
{
    [ExecuteInEditMode]
    public class DevTools : ManagerBase
    {
        public static bool drawPlanets = false;

        public static bool logPlanetInfo = true;


        private void Awake()
        {
            // if (instance != null && instance != this)
            // {
            //     Destroy(gameObject);
            // }
            // else
            // {
            //     instance = this;
            //     if (Application.isPlaying) DontDestroyOnLoad(gameObject);
            // }
        }

        // private void Update()
        // {
        //     if (!Application.isPlaying && instance != null)
        //     {
        //         print("set");
        //         instance = this;
        //     }
        // }
    }
}