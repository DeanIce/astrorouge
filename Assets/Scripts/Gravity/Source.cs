using UnityEngine;

namespace Gravity
{
    public class Source : MonoBehaviour
    {
        protected Rigidbody rb;


        private void OnEnable()
        {
            GravityManager.Register(this);
        }

        private void OnDisable()
        {
            GravityManager.Unregister(this);
        }

        public virtual Vector3 GetGravity(Vector3 position) => Physics.gravity;
    }
}