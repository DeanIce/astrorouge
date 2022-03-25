using System.Collections.Generic;
using UnityEngine;

namespace Planets
{
    public class PlanetCache : MonoBehaviour
    {
        private static readonly Dictionary<int, PlanetGenerator> cache = new();
        private static readonly Dictionary<int, ComputeBuffer> buffers = new();


        public static void Clear()
        {
            cache.Clear();
            buffers.Clear();
        }

        public static void Add(int key, PlanetGenerator pg)
        {
            Debug.Log($"Added planet {key} to cache.");
            cache.Add(key, pg);
            // buffers.Add(key, ComputeHelper.CreateBufferFromJob());
        }
    }
}