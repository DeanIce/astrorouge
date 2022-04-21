using UnityEngine;

namespace Enemy.Ingenalvus
{
    public class IngenalvusAttacks : MonoBehaviour
    {
        public GameObject fireParticles;
        public Animator animator;

        private IngenalvusFire ingFire;

        private void Start()
        {
            animator = GetComponentInChildren<Animator>();
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

        public void Smash()
        {
            animator.SetTrigger("Smash");
        }
    }
}