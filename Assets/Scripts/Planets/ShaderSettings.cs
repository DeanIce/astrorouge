using System;
using UnityEngine;
using Utilities;

namespace Planets
{
    [Serializable]
    public class ShaderSettings
    {
        private const int textureResolution = 50;
        public bool hasOcean;

        public Gradient gradient;

        public EarthColours customizedCols;
        public EarthColours randomizedCols;

        [Range(0, 1)] public float oceanLevel;

        // public OceanSettings oceanSettings;

        public bool randomize;
        public int seed;

        public ComputeShader shadingDataCompute;

        public Material terrainMaterial;
        protected Vector4[] cachedShadingData;
        private ComputeBuffer shadingBuffer;
        private Texture2D texture;

        private ShaderSettings()
        {
        }

        public event Action OnSettingChanged;

        // 
        public virtual void Initialize(ShapeSettings shape)
        {
        }

        // Generate Vector4[] of shading data. This is stored in mesh uvs and used to help shade the body
        public Vector4[] GenerateShadingData(ComputeBuffer vertexBuffer)
        {
            var numVertices = vertexBuffer.count;
            var shadingData = new Vector4[numVertices];

            if (shadingDataCompute)
            {
                // Set data
                SetShadingDataComputeProperties();

                shadingDataCompute.SetInt("numVertices", numVertices);
                shadingDataCompute.SetBuffer(0, "vertices", vertexBuffer);
                ComputeHelper.CreateAndSetBuffer<Vector4>(ref shadingBuffer, numVertices, shadingDataCompute,
                    "shadingData");

                // Run
                ComputeHelper.Run(shadingDataCompute, numVertices);

                // Get data
                shadingBuffer.GetData(shadingData);
            }

            cachedShadingData = shadingData;
            return shadingData;
        }

        // Set shading properties on terrain
        public virtual void SetTerrainProperties(Material material, Vector2 heightMinMax, float bodyScale)
        {
            if (texture == null)
            {
                Debug.Log("recreate texture");
                texture = new Texture2D(textureResolution, 1);
            }

            material.SetVector("_heightMinMax", heightMinMax);
            material.SetFloat("oceanLevel", oceanLevel);
            material.SetFloat("bodyScale", bodyScale);
            Debug.Log(texture);

            var colors = new Color[textureResolution];
            for (var i = 0; i < textureResolution; i++) colors[i] = gradient.Evaluate(i / (textureResolution - 1f));
            texture.SetPixels(colors);
            texture.Apply();
            material.SetTexture("_Texture", texture);

            if (randomize)
            {
                SetRandomColours(material);
                ApplyColours(material, randomizedCols);
            }
            else
            {
                ApplyColours(material, customizedCols);
            }
        }

        private void SetRandomColours(Material material)
        {
            var random = new PRNG(seed);
            //randomizedCols.shoreCol = ColourHelper.Random (random, 0.3f, 0.7f, 0.4f, 0.8f);
            randomizedCols.flatColLowA = ColorHelper.Random(random, 0.45f, 0.6f, 0.7f, 0.8f);
            randomizedCols.flatColHighA = ColorHelper.TweakHSV(
                randomizedCols.flatColLowA,
                random.SignedValue() * 0.2f,
                random.SignedValue() * 0.15f,
                random.Range(-0.25f, -0.2f)
            );

            randomizedCols.flatColLowB = ColorHelper.Random(random, 0.45f, 0.6f, 0.7f, 0.8f);
            randomizedCols.flatColHighB = ColorHelper.TweakHSV(
                randomizedCols.flatColLowB,
                random.SignedValue() * 0.2f,
                random.SignedValue() * 0.15f,
                random.Range(-0.25f, -0.2f)
            );

            randomizedCols.shoreColLow = ColorHelper.Random(random, 0.2f, 0.3f, 0.9f, 1);
            randomizedCols.shoreColHigh = ColorHelper.TweakHSV(
                randomizedCols.shoreColLow,
                random.SignedValue() * 0.2f,
                random.SignedValue() * 0.2f,
                random.Range(-0.3f, -0.2f)
            );

            randomizedCols.steepLow = ColorHelper.Random(random, 0.3f, 0.7f, 0.4f, 0.6f);
            randomizedCols.steepHigh = ColorHelper.TweakHSV(
                randomizedCols.steepLow,
                random.SignedValue() * 0.2f,
                random.SignedValue() * 0.2f,
                random.Range(-0.35f, -0.2f)
            );
        }

        private void ApplyColours(Material material, EarthColours colours)
        {
            material.SetColor("_ShoreLow", colours.shoreColLow);
            material.SetColor("_ShoreHigh", colours.shoreColHigh);

            material.SetColor("_FlatLowA", colours.flatColLowA);
            material.SetColor("_FlatHighA", colours.flatColHighA);

            material.SetColor("_FlatLowB", colours.flatColLowB);
            material.SetColor("_FlatHighB", colours.flatColHighB);

            material.SetColor("_SteepLow", colours.steepLow);
            material.SetColor("_SteepHigh", colours.steepHigh);
        }

        public virtual void SetOceanProperties(Material oceanMaterial)
        {
            // if (oceanSettings) oceanSettings.SetProperties(oceanMaterial, seed, randomize);
        }

        // Override this to set properties on the shadingDataCompute before it is run
        protected virtual void SetShadingDataComputeProperties()
        {
        }

        public virtual void ReleaseBuffers()
        {
            ComputeHelper.Release(shadingBuffer);
        }

        public static void TextureFromGradient(ref Texture2D texture, int width, Gradient gradient,
            FilterMode filterMode = FilterMode.Bilinear)
        {
            if (texture == null)
                texture = new Texture2D(width, 1);
            else if (texture.width != width) texture.Reinitialize(width, 1);
            if (gradient == null)
            {
                gradient = new Gradient();
                gradient.SetKeys(
                    new[] {new(Color.black, 0), new GradientColorKey(Color.black, 1)},
                    new[] {new GradientAlphaKey(1, 0), new GradientAlphaKey(1, 1)}
                );
            }

            texture.wrapMode = TextureWrapMode.Clamp;
            texture.filterMode = filterMode;

            var cols = new Color[width];
            for (var i = 0; i < cols.Length; i++)
            {
                var t = i / (cols.Length - 1f);
                cols[i] = gradient.Evaluate(t);
            }

            texture.SetPixels(cols);
            texture.Apply();
        }

        protected virtual void OnValidate()
        {
            /*
            Shader activeShader = (shader) ? shader : Shader.Find ("Unlit/Color");
            if (material == null || material.shader != activeShader) {
                if (material == null) {
                    material = new Material (activeShader);
                } else {
                    material.shader = activeShader;
                }
            }
            */
            if (OnSettingChanged != null) OnSettingChanged();
        }

        [Serializable]
        public struct EarthColours
        {
            public Color shoreColLow;
            public Color shoreColHigh;
            public Color flatColLowA;
            public Color flatColHighA;
            public Color flatColLowB;
            public Color flatColHighB;

            public Color steepLow;
            public Color steepHigh;
        }
    }
}