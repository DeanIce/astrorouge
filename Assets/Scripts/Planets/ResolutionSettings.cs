using System;
using UnityEngine;

namespace Planets
{
    [Serializable]
    public class ResolutionSettings
    {
        private const int MaxAllowedResolution = 500;
        public const int NumLODLevels = 3;
        public int lod0 = 300;
        public int lod1 = 100;
        public int lod2 = 50;
        public int collider = 100;

        public int GetLODResolution(int lodLevel)
        {
            return lodLevel switch
            {
                0 => lod0,
                1 => lod1,
                2 => lod2,
                _ => lod2
            };
        }

        public void ClampResolutions()
        {
            lod0 = Mathf.Min(MaxAllowedResolution, lod0);
            lod1 = Mathf.Min(MaxAllowedResolution, lod1);
            lod2 = Mathf.Min(MaxAllowedResolution, lod2);
            collider = Mathf.Min(MaxAllowedResolution, collider);
        }
    }
}