using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnenter : MonoBehaviour
{
    public float dmg;
    public float ticDelay;

    private float lastDmg = 0;

    private void Start()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        var temp = other.transform.root.gameObject.GetComponent<PlayerDefault>();
        if (Time.time - lastDmg > ticDelay && temp != null)
        {
            print($"dmg: {dmg}, lastDmg: {lastDmg}, Time-lastDmg: {Time.time - lastDmg}, enabled: {enabled}");
        }
        if (temp != null && Time.time - lastDmg > ticDelay && enabled)
        {
            print("test");
            temp.TakeDmg(dmg);
            lastDmg = Time.time;
        }
    }

}
