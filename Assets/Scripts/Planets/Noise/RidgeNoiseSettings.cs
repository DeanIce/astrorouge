using System;
using UnityEngine;
using Utilities;
using Vector3 = System.Numerics.Vector3;

namespace Planets.Noise
{
    [Serializable]
    public class RidgeNoiseSettings
    {
        public int numLayers = 5;
        public float lacunarity = 2;
        public float persistence = 0.5f;
        public float scale = 1;
        public float power = 2;
        public float elevation = 1;
        public float gain = 1;
        public float verticalShift;
        public float peakSmoothing;
        public Vector3 offset;

        // Set values using exposed settings
        public void SetComputeValues(ComputeShader cs, PRNG prng, string varSuffix)
        {
            SetComputeValues(cs, prng, varSuffix, scale, elevation, power);
        }

        // Set values using custom scale and elevation
        public void SetComputeValues(ComputeShader cs, PRNG prng, string varSuffix, float scale, float elevation,
            float power)
        {
            var seededOffset = new Vector3(prng.Value(), prng.Value(), prng.Value()) * prng.Value() * 10000;

            float[] noiseParams =
            {
                // [0]
                seededOffset.X + offset.X,
                seededOffset.Y + offset.Y,
                seededOffset.Z + offset.Z,
                numLayers,
                // [1]
                persistence,
                lacunarity,
                scale,
                elevation,
                // [2]
                power,
                gain,
                verticalShift,
                peakSmoothing
            };

            cs.SetFloats("noiseParams" + varSuffix, noiseParams);
        }
    }
}