using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaBoss : MonoBehaviour
{
    /*
     * Future Proofing Ideas:
     * 1) Abstract animation stuff to new class
     * 2)
     */

    // Animation stuff
    private Animator animator;

    // Components
    private Rigidbody rb;

    // Omnipotence
    public GameObject player;

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
            StartCoroutine(DeathAnimation());
        }
    }

    // Death
    IEnumerator DeathAnimation()
    {
        animator.SetBool("Dying", true);
        yield return new WaitForSeconds(3);
        animator.SetBool("Dying", false);
    }

    // Attacks
    IEnumerator Roar()
    {
        // Don't set false here, instead set false in followup attacks
        animator.SetBool("Roaring", true);
        yield return new WaitForSeconds(3);
    }

    IEnumerator TongueAttack()
    {
        animator.SetBool("TongueAttacking", true);
        yield return new WaitForSeconds(3);
        animator.SetBool("TongueAttacking", false);
        attacking = false;
    }

    IEnumerator HornAttack()
    {
        animator.SetBool("HornAttacking", true);
        yield return new WaitForSeconds(3);
        animator.SetBool("HornAttacking", false);
        attacking = false;
    }

    IEnumerator RamAttack()
    {
        // Setting roaring false here since we come from roaring and need it to be true to attack
        animator.SetBool("RamAttacking", true);
        yield return new WaitForSeconds(3);
        animator.SetBool("RamAttacking", false);
        animator.SetBool("Roaring", false);
        attacking = false;
    }

    IEnumerator SlamAttack()
    {
        // Setting roaring false here since we come from roaring and need it to be true to attack
        animator.SetBool("SlamAttacking", true);
        yield return new WaitForSeconds(3);
        animator.SetBool("SlamAttacking", false);
        animator.SetBool("Roaring", false);
        attacking = false;
    }
}
