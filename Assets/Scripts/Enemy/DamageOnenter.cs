using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnenter : MonoBehaviour
{
    public float dmg;
    public float ticDelay;

    private float lastDmg = 0;
    private bool doShit = false;

    private void Start()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        var temp = other.transform.root.gameObject.GetComponent<PlayerDefault>();
        if (temp != null && doShit)
        {
            temp.TakeDmg(dmg);
            doShit = false;
        }
    }

    public void EnableDoShit()
    {
        doShit = true;
    }

    public void DisableDoShit()
    {
        doShit = false;
    }

}
