using System;
using UnityEngine;

namespace Planets
{
    [Serializable]
    public class ShaderSettings
    {
        private const int TextureResolution = 50;
        private static readonly int minMaxId = Shader.PropertyToID("_heightMinMax");
        private static readonly int textureId = Shader.PropertyToID("_Texture");

        private static readonly int oceanLevelId = Shader.PropertyToID("oceanLevel");

        public Gradient gradient;

        // Todo: implement
        // public bool hasOcean;
        // public EarthColours customizedCols;
        // public EarthColours randomizedCols;
        // public OceanSettings oceanSettings;

        [Range(0, 1)] public float oceanLevel;


        public Material terrainMaterial;


        private Texture2D texture;

        private ShaderSettings()
        {
        }

        public event Action OnSettingChanged;

        // 
        public virtual void Initialize(ShapeSettings shape)
        {
        }


        // Set shading properties on terrain
        public virtual void SetTerrainProperties(Material material, Vector2 heightMinMax, float bodyScale)
        {
            if (texture == null) texture = new Texture2D(TextureResolution, 1);

            material.SetVector(minMaxId, heightMinMax);
            material.SetFloat(oceanLevelId, oceanLevel);

            var colors = new Color[TextureResolution];
            for (var i = 0; i < TextureResolution; i++) colors[i] = gradient.Evaluate(i / (TextureResolution - 1f));
            texture.SetPixels(colors);
            texture.Apply();
            material.SetTexture(textureId, texture);
        }


        public virtual void SetOceanProperties(Material oceanMaterial)
        {
            // Todo: implement Oceans
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


        // Todo: not used yet
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