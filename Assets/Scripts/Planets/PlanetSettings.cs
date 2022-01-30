using UnityEngine;

namespace Planets
{
    [CreateAssetMenu(menuName = "Planet Settings/Settings Holder")]
    public class PlanetSettings : ScriptableObject
    {
        public ShapeSettings shapeSettings;
    }
}