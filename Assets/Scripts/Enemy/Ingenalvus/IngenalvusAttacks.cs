using System.Collections;
using UnityEngine;

namespace Enemy.Ingenalvus
{
    public class IngenalvusAttacks : MonoBehaviour
    {
        public GameObject fireParticles;
        public Animator animator;

        public GameObject smashParticles;

        public GameObject smashPointLeft;
        public GameObject smashPointRight;
        public IngenalvusSmashDamage smashDamageLeft;
        public IngenalvusSmashDamage smashDamageRight;

        private IngenalvusFire ingFire;

        private void Start()
        {
            animator = GetComponentInChildren<Animator>();
            ingFire = fireParticles.GetComponent<IngenalvusFire>();
            ingFire.Hide();
            smashDamageLeft.gameObject.SetActive(false);
            smashDamageRight.gameObject.SetActive(false);
        }


        public void BreathFireStart() => ingFire.Show();

        public void BreathFireStop() => ingFire.Hide();

        public void Smash()
        {
            if (GetComponent<Ingenalvus>().mode != Ingenalvus.Mode.Dead)
            {
                animator.SetTrigger("Smash");
                smashDamageLeft.gameObject.SetActive(true);
                smashDamageRight.gameObject.SetActive(true);
                smashPointLeft.GetComponent<CapsuleCollider>().enabled = false;
                smashPointRight.GetComponent<CapsuleCollider>().enabled = false;
            }
        }

        public void SmashParticleDamage()
        {
            Vector3 leftPos = smashPointLeft.transform.position;
            leftPos.y = 0.01f;
            Vector3 rightPos = smashPointRight.transform.position;
            rightPos.y = 0.01f;
            GameObject l = Instantiate(smashParticles, leftPos, Quaternion.identity);
            l.SetActive(true);
            GameObject r = Instantiate(smashParticles, rightPos, Quaternion.identity);
            r.SetActive(true);
            StartCoroutine(DestroyLater(l, r));
        }

        private IEnumerator DestroyLater(GameObject a, GameObject b)
        {
            yield return new WaitForSeconds(5);
            Destroy(a);
            Destroy(b);
            smashDamageLeft.gameObject.SetActive(false);
            smashDamageRight.gameObject.SetActive(false);
            // Todo: actually prevent the player from being shoved below ground
            smashPointLeft.GetComponent<CapsuleCollider>().enabled = true;
            smashPointRight.GetComponent<CapsuleCollider>().enabled = true;
        }
    }
}