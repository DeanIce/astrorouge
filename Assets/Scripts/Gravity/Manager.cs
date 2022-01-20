using UnityEngine;
using System.Collections.Generic;

namespace Gravity
{
    public static class Manager
    {
        private static readonly List<Source> sources = new();

        public static void Register(Source source)
        {
            Debug.AssertFormat(
                !sources.Contains(source),
                "Duplicate registration of gravity source!", source
            );
            sources.Add(source);
        }

        public static void Unregister(Source source)
        {
            Debug.AssertFormat(
                sources.Contains(source),
                "Unregistration of unknown gravity source!", source
            );
            sources.Remove(source);
        }

        public static Vector3 GetGravity(Vector3 position)
        {
            Vector3 g = Vector3.zero;
            foreach (var t in sources)
            {
                g += t.GetGravity(position);
            }

            return g;
        }

        public static Vector3 GetGravity(Vector3 position, out Vector3 upAxis)
        {
            Vector3 g = Vector3.zero;
            foreach (var t in sources)
            {
                g += t.GetGravity(position);
            }

            upAxis = -g.normalized;
            return g;
        }

        public static Vector3 GetUpAxis(Vector3 position)
        {
            Vector3 g = Vector3.zero;
            foreach (var t in sources)
            {
                g += t.GetGravity(position);
            }

            return -g.normalized;
        }
    }
}