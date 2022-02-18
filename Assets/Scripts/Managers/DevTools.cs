using UnityEngine;

namespace Managers
{
    [ExecuteInEditMode]
    public class DevTools : ManagerBase<DevTools>
    {
        public static bool drawPlanets = false;

        public static bool logPlanetInfo = true;
    }
}