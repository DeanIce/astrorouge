using UnityEngine;

namespace Managers
{
    [ExecuteInEditMode]
    public class DevTools : ManagerSingleton<DevTools>
    {
        public static bool drawPlanets = true;

        public static bool logPlanetInfo = true;
    }
}