using UnityEngine;

namespace Enemy.Ingenalvus
{
    /// <summary>
    ///     Attached to the GameObject that holds particle
    ///     system and collider for the fire breathe attack.
    /// </summary>
    public class IngenalvusFire : MonoBehaviour
    {
        private CapsuleCollider coll;
        private Ingenalvus ing;
        private ParticleSystem particles;


        private void Start()
        {
            ing = transform.root.gameObject.GetComponent<Ingenalvus>();
            particles = GetComponent<ParticleSystem>();
            coll = GetComponent<CapsuleCollider>();
        }


        private void OnTriggerStay(Collider other)
        {
            other.gameObject.GetComponent<PlayerDefault>()?.TakeDmg(ing.fireDamage * Time.fixedDeltaTime);
        }

        private void OnTriggerExit(Collider other)
        {
            other.gameObject.GetComponent<StatusEffectManager>().ApplyBurn(3);
        }

        public void Show()
        {
            particles.Play();
            coll.enabled = true;
        }

        public void Hide()
        {
            particles.Stop();
            coll.enabled = false;
        }
    }
}