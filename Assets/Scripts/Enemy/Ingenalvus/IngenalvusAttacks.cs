using UnityEngine;

namespace Enemy.Ingenalvus
{
    public class IngenalvusAttacks : MonoBehaviour
    {
        public GameObject fireParticles;

        private IngenalvusFire ingFire;

        private void Start()
        {
            ingFire = fireParticles.GetComponent<IngenalvusFire>();
            ingFire.Hide();
        }


        public void BreathFireStart()
        {
            ingFire.Show();
        }

        public void BreathFireStop()
        {
            ingFire.Hide();
        }
    }
}