using System;
using Editor;
using Planets.Noise;
using UnityEngine;

namespace Planets
{
    [Serializable]
    public class ShapeSettings
    {
        public ComputeShader heightMapCompute;
        public bool perturbVertices;
        public ComputeShader perturbCompute;

        [Range(0, 1)] public float perturbStrength = 0.7f;

        public int seed;
        public Vector4 testParams;

        [Header("Continent settings")] // 
        public float oceanDepthMultiplier = 5;

        public float oceanFloorDepth = 1.5f;
        public float oceanFloorSmoothing = 0.5f;
        public float mountainBlend = 1.2f; // Determines how smoothly the base of mountains blends into the terrain


        [Header("Noise settings")] // 
        public SimpleNoiseSettings continentNoise;

        public SimpleNoiseSettings maskNoise;
        public RidgeNoiseSettings ridgeNoise;

        private ComputeBuffer heightBuffer;

        // public RidgeNoiseSettings ridgeNoise;

        protected virtual void OnValidate()
        {
            OnSettingChanged?.Invoke();
        }

        public event Action OnSettingChanged;

        public virtual float[] CalculateHeights(ComputeBuffer vertexBuffer)
        {
            //Debug.Log (System.Environment.StackTrace);
            // Set data
            SetShapeData();
            heightMapCompute.SetInt("numVertices", vertexBuffer.count);
            heightMapCompute.SetBuffer(0, "vertices", vertexBuffer);
            ComputeHelper.CreateAndSetBuffer<float>(ref heightBuffer, vertexBuffer.count, heightMapCompute, "heights");

            // Run
            ComputeHelper.Run(heightMapCompute, vertexBuffer.count);

            // Get heights
            var heights = new float[vertexBuffer.count];
            heightBuffer.GetData(heights);
            return heights;
        }

        public virtual void ReleaseBuffers()
        {
            ComputeHelper.Release(heightBuffer);
        }

        protected virtual void SetShapeData()
        {
            var prng = new PRNG(seed);
            continentNoise.SetComputeValues(heightMapCompute, prng, "_continents");
            ridgeNoise.SetComputeValues(heightMapCompute, prng, "_mountains");
            maskNoise.SetComputeValues(heightMapCompute, prng, "_mask");

            heightMapCompute.SetFloat("oceanDepthMultiplier", oceanDepthMultiplier);
            heightMapCompute.SetFloat("oceanFloorDepth", oceanFloorDepth);
            heightMapCompute.SetFloat("oceanFloorSmoothing", oceanFloorSmoothing);
            heightMapCompute.SetFloat("mountainBlend", mountainBlend);
            heightMapCompute.SetVector("params", testParams);
        }
    }
}