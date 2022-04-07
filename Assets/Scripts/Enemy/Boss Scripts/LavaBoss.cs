using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaBoss : MonoBehaviour
{
    // Animation stuff
    private Animator animator;

    // Components
    private Rigidbody rb;

    // Status stuff
    private bool dying;
    private bool attacking;
    public float health;
    public float movementSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // May need to be GetComponentInChildren
        animator = GetComponent<Animator>();
        dying = false;
    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {
        
    }

    public void Die()
    {
        if (!dying)
        {
            dying = true;
            //StartCoroutine(DeathAnimation(1));
        }
    }
    
    // Death
    /*
    IEnumerator DeathAnimation(int anim)
    {
        
    }

    // Attacks
    IEnumerator Roar()
    {

    }

    IEnumerator TongueAttack()
    {

    }

    IEnumerator HornAttack()
    {

    }

    IEnumerator RamAttack()
    {

    }

    IEnumerator SlamAttack()
    {

    }*/
}
