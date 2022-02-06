using System;
using UnityEngine;

namespace Planets
{
    [Serializable]
    public class ResolutionSettings
    {
        private const int MaxAllowedResolution = 500;
        public int lod0 = 300;
        public int lod1 = 100;
        public int lod2 = 50;
        public int collider = 100;

        public int GetLODResolution(int lodLevel)
        {
            switch (lodLevel)
            {
                case 0:
                    return lod0;
                case 1:
                    return lod1;
                case 2:
                    return lod2;
            }

            return lod2;
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