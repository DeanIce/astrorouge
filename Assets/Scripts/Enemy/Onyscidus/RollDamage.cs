using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollDamage : MonoBehaviour
    {
        private IceBoss oni;


        private void Start()
        {
            oni = transform.root.gameObject.GetComponent<IceBoss>();
        }


        private void OnTriggerEnter(Collider other)
        {
            // print("roll collider");
            other.gameObject.GetComponent<PlayerDefault>()?.TakeDmg(oni.attackDamage);
            oni.GetComponent<Animator>().SetBool("Rolling", false);
            oni.rolling = false;
            oni.RollCollider.isTrigger = false;
        }
    }
