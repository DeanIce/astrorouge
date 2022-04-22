using UnityEngine;

namespace Enemy.Ingenalvus
{
    public class IngenalvusSmash : MonoBehaviour
    {
        private BoxCollider coll;
        private IngenalvusAttacks ia;

        private int playerMask;

        private void Start()
        {
            ia = transform.root.gameObject.GetComponent<IngenalvusAttacks>();
            coll = GetComponent<BoxCollider>();
            playerMask = LayerMask.NameToLayer("Player");
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                print("attack triggered");
                ia.Smash();
            }
        }
    }
}