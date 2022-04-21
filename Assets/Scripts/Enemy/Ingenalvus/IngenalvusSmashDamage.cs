using UnityEngine;

namespace Enemy.Ingenalvus
{
    public class IngenalvusSmashDamage : MonoBehaviour
    {
        private Ingenalvus ing;


        private void Start()
        {
            ing = transform.root.gameObject.GetComponent<Ingenalvus>();
        }


        private void OnTriggerEnter(Collider other)
        {
            print(other.gameObject.name);
            other.gameObject.GetComponent<PlayerDefault>()?.TakeDmg(ing.smashDamage);
        }
    }
}