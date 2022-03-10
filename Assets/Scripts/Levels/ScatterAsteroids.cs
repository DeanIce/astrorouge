using System;
using GD.MinMaxSlider;
using UnityEngine;
using Random = System.Random;

namespace Levels
{
    public class ScatterAsteroids : MonoBehaviour
    {
        public Debris[] debris = Array.Empty<Debris>();


        public int seed;

        [MinMaxSlider(0, 1000)] public Vector2 minMaxRadius = new(100, 150);

        private void Start()
        {
            var rng = new Random(seed);

            foreach (var trash in debris)
            {
                var actualCount = rng.Next(trash.count.x, trash.count.y);
                for (var i = 0; i < actualCount; i++)
                {
                    var radius = rngRange(rng, minMaxRadius.x, minMaxRadius.y);
                    var pos = radius * pointOnUnitSphere(rng);
                    var d = Instantiate(trash.prefab, pos, Quaternion.identity);
                    d.transform.parent = transform;
                }
            }
        }


        private Vector3 pointOnUnitSphere(Random rng)
        {
            var v = new Vector3((float) NextGaussian(rng), (float) NextGaussian(rng), (float) NextGaussian(rng));
            v.Normalize();
            return v;
        }

        private float rngRange(Random rng, float start, float end)
        {
            var sample = rng.NextDouble();
            var scaled = sample * (end - start) + start;
            var f = (float) scaled;
            return f;
        }

        /// <summary>
        ///     Generates normally distributed numbers. Each operation makes two Gaussians for the price of one, and apparently
        ///     they can be cached or something for better performance, but who cares.
        /// </summary>
        /// <param name="r"></param>
        /// <param name="mu">Mean of the distribution</param>
        /// <param name="sigma">Standard deviation</param>
        /// <returns></returns>
        public static double NextGaussian(Random r, double mu = 0, double sigma = 1)
        {
            var u1 = r.NextDouble();
            var u2 = r.NextDouble();

            var rand_std_normal = Math.Sqrt(-2.0 * Math.Log(u1)) *
                                  Math.Sin(2.0 * Math.PI * u2);

            var rand_normal = mu + sigma * rand_std_normal;

            return rand_normal;
        }


        [Serializable]
        public class Debris
        {
            public GameObject prefab;

            [MinMaxSlider(0, 10)] public Vector2 minMaxScale = new(1, 2);

            [MinMaxSlider(0, 10)] public Vector2Int count = new(1, 3);
        }
    }
}