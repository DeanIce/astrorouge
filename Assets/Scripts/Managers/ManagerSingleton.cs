using System;
using UnityEngine;

namespace Managers
{
    public abstract class ManagerSingleton<T> : MonoBehaviour where T : ManagerSingleton<T>
    {
        // private static readonly Lazy<T> lazyInstance = new(() =>
        // {
        //     var a = FindObjectOfType<T>();
        //     return a;
        // });


        public bool logEvents = true;

        public static T Instance { get; private set; }
        // public static T Instance => lazyInstance.Value;

        // private set => lazyInstance = value;
        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(gameObject);
            else
            {
                Instance = this as T;
                if (Application.isPlaying) DontDestroyOnLoad(gameObject);
            }
        }

        /// <summary>
        ///     Logs message to the Unity Console, prefaced by manager name.
        /// </summary>
        /// <param name="message"></param>
        protected void LOG(object message)
        {
            Type type = GetType().UnderlyingSystemType;
            string className = type.Name;
            if (logEvents) print($"{className}: {message}");
        }
    }
}