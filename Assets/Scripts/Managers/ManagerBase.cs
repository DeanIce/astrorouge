using UnityEngine;

namespace Managers
{
    public abstract class ManagerBase : MonoBehaviour
    {
        public bool logEvents = true;

        protected void log(object o)
        {
            if (logEvents) print($"EventManager: {o}");
        }
    }
}