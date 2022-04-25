using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace Enemy.Ingenalvus
{
    public class IngenalvusAttacks : MonoBehaviour
    {
        private static readonly int smash = Animator.StringToHash("Smash");
        public GameObject fireParticles;
        public Animator animator;

        public GameObject smashParticles;

        [FormerlySerializedAs("smashPointLeft")]
        public GameObject footLeftFront;

        [FormerlySerializedAs("smashPointRight")]
        public GameObject footRightFront;

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
                animator.SetTrigger(smash);
                smashDamageLeft.gameObject.SetActive(true);
                smashDamageRight.gameObject.SetActive(true);
                footLeftFront.GetComponent<CapsuleCollider>().enabled = false;
                footRightFront.GetComponent<CapsuleCollider>().enabled = false;
            }
        }

        public void SmashParticleDamage()
        {
            Vector3 leftPos = footLeftFront.transform.position;
            leftPos.y = 0.01f;
            Vector3 rightPos = footRightFront.transform.position;
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
            footLeftFront.GetComponent<CapsuleCollider>().enabled = true;
            footRightFront.GetComponent<CapsuleCollider>().enabled = true;
        }
    }
}