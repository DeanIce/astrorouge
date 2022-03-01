using UnityEngine;

namespace Gravity
{
    public class SphereSource : Source
    {
        public float gravity = 9.81f;

        [Min(0f)] public float outerRadius = 10f, outerFalloffRadius = 15f;

        private float outerFalloffFactor;

        private void Awake()
        {
            OnValidate();
        }

        private void OnDrawGizmos()
        {
            var p = transform.position;
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(p, outerRadius);
            if (outerFalloffRadius > outerRadius)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireSphere(p, outerFalloffRadius);
            }
        }

        private void OnValidate()
        {
            outerFalloffRadius = Mathf.Max(outerFalloffRadius, outerRadius);

            outerFalloffFactor = 1f / (outerFalloffRadius - outerRadius);
        }

        public override Vector3 GetGravity(Vector3 position)
        {
            var vector = transform.position - position;
            var distance = vector.magnitude;
            if (distance > outerFalloffRadius) return Vector3.zero;

            var g = gravity / distance;
            if (distance > outerRadius) g *= 1f - (distance - outerRadius) * outerFalloffFactor;

            print(rb);
            var m = 1.0f;
            if (rb != null) m = rb.mass;

            return vector * (g * m);
        }
    }
}