﻿using UnityEngine;

namespace Levels
{
    public class BallDropper : MonoBehaviour
    {
        private static GameObject self;

        private void Start()
        {
            self = gameObject;
        }

        /// <summary>
        /// </summary>
        /// <param name="radii">sorted biggest to smallest</param>
        /// <returns></returns>
        public static Vector3[] DropBalls(float[] radii)
        {
            var numSteps = 5000;
            var stepSize = .01f;
            var result = new Vector3[radii.Length];

            var maxRadius = radii[0];
            Physics.autoSimulation = false;

            var balls = new GameObject[radii.Length];

            for (var i = 0; i < radii.Length; i++)
            {
                var newBall = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                newBall.transform.localScale = Vector3.one * radii[i] / maxRadius;
                // newBall.transform.parent = self.transform;
                newBall.transform.position = Vector3.up * i * 2 /*+ Vector3.right * i * .1f*/;
                newBall.AddComponent<Rigidbody>();
                balls[i] = newBall;
            }

            // Physics.autoSimulation = true;

            for (var i = 0; i < numSteps; i++)
            {
                Physics.Simulate(stepSize);
            }


            var lowest = result[0];
            for (var i = 0; i < radii.Length; i++)
            {
                result[i] = balls[i].transform.position * maxRadius * 2;

                // Center the lowest planet at zero-zero-zero
                if (result[i].y < lowest.y) lowest = result[i];

                // Destroy the test ball
                DestroyImmediate(balls[i]);
            }

            // Move all spheres to fit the lowest sphere at (0,0,0)
            for (var i = 0; i < radii.Length; i++)
            {
                result[i] -= lowest;
            }


            Physics.autoSimulation = true;

            return result;
        }
    }
}