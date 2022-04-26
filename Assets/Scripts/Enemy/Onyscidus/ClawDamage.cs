using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawDamage : MonoBehaviour
    {
        private IceBoss oni;


        private void Start()
        {
            oni = transform.root.gameObject.GetComponent<IceBoss>();
        }


        private void OnTriggerEnter(Collider other)
        {
            // print("claw collider");
            other.transform.root.gameObject.GetComponent<PlayerDefault>()?.TakeDmg(oni.attackDamage);

            // chance for slow effect
            if (Random.value < 0.3) {
                var slow = new SlowEffect();
                slow.ApplyEffect(other.transform.root.gameObject);
            }
        }
    }
