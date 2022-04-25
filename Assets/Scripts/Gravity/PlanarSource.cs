using UnityEngine;

namespace Gravity
{
    public class PlanarSource : Source
    {
        public float gravity = 9.81f;
        public float multiplier = 100;

        private BoxCollider box;

        private void Awake()
        {
            OnValidate();
        }

        private void Start()
        {
            box = GetComponent<BoxCollider>();
        }

        private void OnDrawGizmos()
        {
        }

        private void OnValidate()
        {
        }


        public override Vector3 GetGravity(Vector3 position)
        {
            // print(box.bounds);
            if (box.bounds.Contains(position)) return Vector3.down * gravity * multiplier;

            return Vector3.zero;
        }
    }
}