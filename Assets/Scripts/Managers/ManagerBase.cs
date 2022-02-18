using UnityEngine;

namespace Managers
{
    public abstract class ManagerBase<T> : MonoBehaviour where T : Component
    {
        public bool logEvents = true;

        public static T instance { get; private set; }

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this as T;
                if (Application.isPlaying)
                {
                    DontDestroyOnLoad(gameObject);
                }
            }
        }

        /// <summary>
        ///     Logs message to the Unity Console, prefaced by manager name.
        /// </summary>
        /// <param name="message"></param>
        protected void LOG(object message)
        {
            var type = GetType().UnderlyingSystemType;
            var className = type.Name;
            if (logEvents)
            {
                print($"{className}: {message}");
            }
        }
    }
}