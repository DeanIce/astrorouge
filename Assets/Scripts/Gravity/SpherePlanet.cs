using Planets;
using UnityEngine;

namespace Gravity
{
    [ExecuteAlways]
    public class SpherePlanet : MonoBehaviour
    {
        public float size = 5f;

        private readonly string childName = "child";

        private Transform childTransform;

        private PlanetGenerator planetGenerator;


        // Start is called before the first frame update
        private void Start()
        {
            childTransform = transform.Find(childName);
            if (childTransform == null)
            {
                var resource = Resources.Load("PlanetTemplateSphere") as GameObject;
                var child = Instantiate(resource, transform.position, transform.rotation);
                child.name = childName;
                childTransform = child.transform;
                childTransform.parent = transform;
            }

            planetGenerator = GetComponent<PlanetGenerator>();
        }

        // Update is called once per frame
        private void Update()
        {
            if (childTransform == null) childTransform = transform.Find(childName);
            childTransform.localScale = new Vector3(size, size, size);

            if (planetGenerator) planetGenerator.scale = size / 2f;
        }
    }
}