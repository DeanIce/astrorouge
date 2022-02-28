using UnityEngine;

namespace Utilities
{
    public class MinMaxSliderAttribute : PropertyAttribute
    {
        public float max;

        public float min;

        public MinMaxSliderAttribute(float min, float max)
        {
            this.min = min;
            this.max = max;
        }
    }
}