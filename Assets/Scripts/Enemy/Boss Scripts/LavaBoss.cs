using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
    private bool hunting;
    private bool inRange;
    public float health;
    public float movementSpeed;

    // Movement stuff
    private NavMeshAgent navMeshAgent;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // May need to be GetComponentInChildren
        animator = GetComponent<Animator>();
        dying = false;
        inRange = false;
        attacking = false;
    }

    void Update()
    {
        if (!inRange)
        {
            animator.SetBool("Crawling", true);
            navMeshAgent.isStopped = false;
            navMeshAgent.destination = player.transform.position;
        }
        else
        {
            navMeshAgent.isStopped = true;
            animator.SetBool("Crawling", false);
        }
    }

    void FixedUpdate()
    {
        
    }

    // For detecting if the player is within a reasonable attacking range
    void OnTriggerEnter(Collider other)
    {
        // Convention: Player layer is 9
        if (other.gameObject.layer == 9)
        {
            Debug.Log("In Range!");
            inRange = true;
        }
    }

    // For detecting if the player leaves the reasonable attacking range
    private void OnTriggerExit(Collider other)
    {
        // Convention: Player layer is 9
        if (other.gameObject.layer == 9)
        {
            Debug.Log("Left Range!");
            inRange = false;
        }
    }

    public void Die()
    {
        if (!dying)
        {
            dying = true;
            StartCoroutine(DeathAnimation());
        }
    }

    // Movement
    // IEnums for crawl/rotate?

    // Damage Taken
    // TODO: Alter timings to match animation speeds
    IEnumerator DamageLevel1()
    {
        animator.SetBool("Destroyed1", true);
        yield return new WaitForSeconds(3);
        animator.SetBool("Destroyed1", false);
    }

    IEnumerator DamageLevel2()
    {
        animator.SetBool("Destroyed2", true);
        yield return new WaitForSeconds(3);
        animator.SetBool("Destroyed2", false);
    }

    IEnumerator DamagedRoar()
    {
        animator.SetBool("DamagedRoar", true);
        yield return new WaitForSeconds(3);
        animator.SetBool("DamagedRoar", false);
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
        yield return new WaitForSeconds(5);
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
