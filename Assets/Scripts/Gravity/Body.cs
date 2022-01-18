using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Gravity
{
    public class Body : MonoBehaviour
    {
        public GameObject attractorObject;
        private Attractor attractor;

        private Rigidbody rb;
        public float maxSpeed = 10f;
        public float jumpForce = 3f;
        
        
        Vector3 velocity, desiredVelocity;
        bool desiredJump;

        // Start is called before the first frame update
        void Start()
        {
            attractor = attractorObject.GetComponent<Attractor>();
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
            attractor.Attract(gameObject);


            float strafe = Input.GetAxis("Horizontal");
            float walk = Input.GetAxis("Vertical");


            var force = -transform.right * walk + transform.forward * strafe;
            // if (desiredJump)
            // {
            //     force += Vector3.up * jumpForce;
            // }

            rb.AddForce(force*maxSpeed);
        }
    }
}