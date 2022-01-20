using UnityEngine;

namespace Gravity
{
    public class Source : MonoBehaviour
    {
        public Rigidbody rb;

        private void Start()
        {
            rb = GetComponentInChildren<Rigidbody>();
        }

        public virtual Vector3 GetGravity(Vector3 position)
        {
            return Physics.gravity;
        }

        void OnEnable()
        {
            Manager.Register(this);
        }

        void OnDisable()
        {
            Manager.Unregister(this);
        }
    }
}