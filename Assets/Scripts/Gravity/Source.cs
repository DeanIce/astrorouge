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

        private void OnEnable()
        {
            GravityManager.Register(this);
        }

        private void OnDisable()
        {
            GravityManager.Unregister(this);
        }

        public virtual Vector3 GetGravity(Vector3 position)
        {
            return Physics.gravity;
        }
    }
}