using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Gravity
{
    public class Body : MonoBehaviour
    {
        public List<GameObject> attractorObjects;
        private readonly List<Attractor> attractors = new();
        public float turningRate = 70f;

        private Rigidbody rb;
        public float maxSpeed = 10f;
        public float jumpForce = 3f;


        private Vector3 sumForce = Vector3.zero;
        private Vector3 upAxis;
        Vector3 velocity;
        bool desiredJump;
        
        

        // Start is called before the first frame update
        void Start()
        {
            foreach (var attractorObject in attractorObjects)
            {
                attractors.Add(attractorObject.GetComponent<Attractor>());
            }

            rb = GetComponent<Rigidbody>();
        }


        private void Update()
        {
            desiredJump |= Input.GetButtonDown("Jump");
        }


        
        void FixedUpdate()
        {
            // Gravity
            sumForce = Manager.GetGravity(transform.position, out upAxis);
            rb.AddForce(sumForce * Time.deltaTime);
            Debug.DrawLine(transform.position, sumForce, Color.blue);
            rb.MoveRotation(Quaternion.FromToRotation(transform.up, upAxis) * transform.rotation);

            // Naive movement code
            float strafe = Input.GetAxis("Horizontal");
            float walk = Input.GetAxis("Vertical");
            var force = -transform.right * walk + transform.forward * strafe;
            rb.AddForce(force * maxSpeed);
            
            // Bad jump code
            if (desiredJump)
            {
                rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
                desiredJump = false;
            }
        }
    }
}