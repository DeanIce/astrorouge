using UnityEngine;

namespace Managers
{
    public abstract class ManagerBase : MonoBehaviour
    {
        public bool logEvents = true;

        protected void LOG(object o)
        {
            if (logEvents) print($"EventManager: {o}");
        }
    }
}