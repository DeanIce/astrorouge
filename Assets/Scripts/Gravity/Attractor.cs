using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Gravity
{
    public class Attractor : MonoBehaviour
    {
        private Vector3 center;
        public float gForce = -9.8f;
        public float turningRate = 30f;

        public float limit = 10f;

        private Rigidbody rb;

        private Quaternion target = Quaternion.identity;

        private Vector3 lastNorm = Vector3.zero;

        Vector3[] casts = new Vector3[10];


        // Start is called before the first frame update
        void Start()
        {
            center = transform.position;
            rb = GetComponentInChildren<Rigidbody>();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            // Attract(item);
        }

        private Vector3 GetMeanVector(Vector3[] positions)
        {
            if (positions.Length == 0)
                return Vector3.zero;
            float x = 0f;
            float y = 0f;
            float z = 0f;
            foreach (Vector3 pos in positions)
            {
                x += pos.x;
                y += pos.y;
                z += pos.z;
            }

            return new Vector3(x / positions.Length, y / positions.Length, z / positions.Length);
        }

        Vector3 FindSurface(GameObject o)
        {
            var surfaceNormal = Vector3.zero;

            float distance = Vector3.Distance(transform.position, o.transform.position);

            // if (distance > limit)
            // {
            //     return lastNorm;
            //     return Vector3.zero;
            // }
            
            //
            // Debug.DrawLine(o.transform.position,center, Color.yellow);
            if (Physics.Raycast(o.transform.position, center-o.transform.position, out var hit))
            {
                // print("hit");
                surfaceNormal = hit.normal;
                Debug.DrawLine(hit.point, hit.point + hit.normal, Color.red);
            }

            // surfaceNormal = Raycaster.DoRayCast(o, 5, 5, .2f);

            // lastNorm = surfaceNormal;
            return surfaceNormal;
        }

        Quaternion OrientBody(GameObject o, Vector3 surfaceNormal)
        {
            // target = Quaternion.FromToRotation(o.transform.up, surfaceNormal) * o.transform.rotation;
            //
            //
            // o.transform.localRotation =
            //     Quaternion.RotateTowards(o.transform.rotation, target, turningRate * Time.deltaTime);
            // // o.transform.localRotation = target;
            return Quaternion.FromToRotation(o.transform.up, surfaceNormal) * o.transform.rotation;
        }


        public (Vector3, Vector3, bool) Attract(GameObject o)
        {
            
            
            float distance = Vector3.Distance(transform.position, o.transform.position);
            
            
            
            // print(o.name);
            // print(o.transform.position);

            Vector3 pullVec = FindSurface(o);
            var quat = OrientBody(o, pullVec);

            // F = G*M*m / r^2
            float M = rb.mass;
            var oRb = o.GetComponentInChildren<Rigidbody>();
            float m = oRb.mass;

            var r = Vector3.Distance(transform.position, o.transform.position);
            float pullForce = gForce * M * m / (float) Math.Pow(r, 2);

            Vector3 force = pullVec.normalized * pullForce;

            // pullVec = o.transform.position - center;
            // oRb.AddForce(force * Time.deltaTime);

            return (pullVec, force, distance < limit);
        }    
        void OnDrawGizmos()
        {
            // Gizmos.matrix = transform.localToWorldMatrix;
            
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(center,limit);
        }
    }
    
    

}