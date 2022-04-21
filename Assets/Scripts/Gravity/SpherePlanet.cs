using Planets;
using UnityEngine;

namespace Gravity
{
    [ExecuteAlways]
    public class SpherePlanet : MonoBehaviour
    {
        private const string ChildName = "child";
        public float size = 5f;

        private Transform childTransform;

        private PlanetGenerator planetGenerator;


        private void Start()
        {
            childTransform = transform.Find(ChildName);
            if (childTransform == null)
            {
                var resource = Resources.Load("PlanetTemplateSphere") as GameObject;
                Transform thisTransform = transform;
                GameObject child = Instantiate(resource, thisTransform.position, thisTransform.rotation);
                child.name = ChildName;
                childTransform = child.transform;
                childTransform.parent = transform;
            }

            planetGenerator = GetComponent<PlanetGenerator>();
        }

        private void Update()
        {
            if (childTransform == null) childTransform = transform.Find(ChildName);

            childTransform.localScale = new Vector3(size, size, size);

            if (planetGenerator) planetGenerator.scale = size / 2f;
        }
    }
}