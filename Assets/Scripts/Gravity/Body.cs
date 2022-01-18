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


        Vector3 velocity, desiredVelocity;
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
            Vector2 playerInput;
            playerInput.x = Input.GetAxis("Horizontal");
            playerInput.y = Input.GetAxis("Vertical");
            playerInput = Vector2.ClampMagnitude(playerInput, 1f);

            desiredVelocity =
                new Vector3(playerInput.x, 0f, playerInput.y) * maxSpeed;

            desiredJump |= Input.GetButtonDown("Jump");
        }

        void FixedUpdate()
        {
            Quaternion sumQuat = Quaternion.identity;
            Vector3 sumRot = Vector3.zero;
            Vector3 sumForce = Vector3.zero;

            foreach (var a in attractors)
            {
                var (quat, f) = a.Attract(gameObject);
                sumRot += quat;
                sumForce += f;
            }

            sumForce /= attractors.Count;

            Debug.DrawLine(transform.position, sumForce, Color.blue);

            // Apply the combined rotations
            transform.localRotation = Quaternion.FromToRotation(transform.up, sumRot) * transform.rotation;
            // oRb.AddForce(force * Time.deltaTime);


            float strafe = Input.GetAxis("Horizontal");
            float walk = Input.GetAxis("Vertical");


            var force = -transform.right * walk + transform.forward * strafe;
            rb.AddForce(force * maxSpeed);

            if (desiredJump)
            {
                print("jump");
                rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
                // rb.velocity += transform.up * jumpForce;
                desiredJump = false;
            }
        }
    }
}