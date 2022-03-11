using UnityEngine;

namespace Gravity
{
    public class PlanarSource : Source
    {
        public float gravity = 9.81f;
        public float multiplier = 100;


        private void Awake()
        {
            OnValidate();
        }

        private void OnDrawGizmos()
        {
        }

        private void OnValidate()
        {
        }

        public override Vector3 GetGravity(Vector3 position)
        {
            return Vector3.down * gravity * multiplier;
        }
    }
}