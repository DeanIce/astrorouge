using UnityEngine;

namespace Managers
{
    public abstract class ManagerBase : MonoBehaviour
    {
        public bool logEvents = true;

        /// <summary>
        ///     Logs message to the Unity Console, prefaced by manager name.
        /// </summary>
        /// <param name="message"></param>
        protected void LOG(object message)
        {
            var type = GetType().UnderlyingSystemType;
            var className = type.Name;
            if (logEvents) print($"{className}: {message}");
        }
    }
}