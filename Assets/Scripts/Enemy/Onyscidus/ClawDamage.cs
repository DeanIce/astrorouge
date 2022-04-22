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
            print("claw collider");
            other.gameObject.GetComponent<PlayerDefault>()?.TakeDmg(oni.attackDamage);
        }
    }
