using UnityEngine;

namespace Utilities
{
    /// <summary>
    /// https://github.com/GucioDevs/SimpleMinMaxSlider
    /// </summary>
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