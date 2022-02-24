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


        // Start is called before the first frame update
        private void Start()
        {
            childTransform = transform.Find(ChildName);
            if (childTransform == null)
            {
                var resource = Resources.Load("PlanetTemplateSphere") as GameObject;
                var thisTransform = transform;
                var child = Instantiate(resource, thisTransform.position, thisTransform.rotation);
                child.name = ChildName;
                childTransform = child.transform;
                childTransform.parent = transform;
            }

            planetGenerator = GetComponent<PlanetGenerator>();
        }

        // Update is called once per frame
        private void Update()
        {
            if (childTransform == null)
            {
                childTransform = transform.Find(ChildName);
            }

            childTransform.localScale = new Vector3(size, size, size);

            if (planetGenerator)
            {
                planetGenerator.scale = size / 2f;
            }
        }
    }
}