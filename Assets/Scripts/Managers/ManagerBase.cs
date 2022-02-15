using UnityEngine;

namespace Managers
{
    public abstract class ManagerBase : MonoBehaviour
    {
        public bool logEvents = true;

        protected void LOG(object o)
        {
            var type = GetType().UnderlyingSystemType;
            var className = type.Name;
            if (logEvents) print($"{className}: {o}");
        }
    }
}