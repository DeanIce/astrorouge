using UnityEngine;

namespace Enemy.Ingenalvus
{
    public class IngenalvusCollider : MonoBehaviour
    {
        public Ingenalvus ingenalvus;

        public bool acceptingDamage;
        public GameObject particles;


        private LayerMask mask = LayerMask.GetMask();

        private void Start()
        {
            particles.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (acceptingDamage)
            {
                ingenalvus.DestroyWeakPoint(this);
                particles.SetActive(true);
                Destroy(other.gameObject);
            }
        }

        public void PassThroughDamage(float dmg)
        {
            print(dmg);
        }
    }
}