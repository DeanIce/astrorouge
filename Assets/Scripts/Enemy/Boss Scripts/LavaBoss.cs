using Managers;
using System.Collections;
using UI;
using UnityEngine;
using UnityEngine.AI;

public class LavaBoss : MonoBehaviour
{
    /*
     * Future Proofing Ideas:
     * 1) Abstract animation stuff to new class
     * 2)
     */

    public BossHealthBar bossHealthBar;

    // Omnipotence
    public GameObject player;
    public float maxHealth;
    public float movementSpeed;

    // Animation stuff
    private Animator animator;
    private bool attacking;

    // Portal
    public GameObject portal;

    // Status stuff
    private bool dying;
    private float health;
    private bool hunting;
    private bool inRange;

    // Misc
    public int expAmt;

    // Movement stuff
    private NavMeshAgent navMeshAgent;

    // Components
    private Rigidbody rb;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        health = maxHealth;
        bossHealthBar.SetHealth(health, maxHealth);
        rb = GetComponent<Rigidbody>();

        // May need to be GetComponentInChildren
        portal.SetActive(false);
        animator = GetComponent<Animator>();
        dying = false;
        inRange = false;
        attacking = false;
    }

    private void Update()
    {
        // Die?
        if (health < 0)
        {
            Die();
        }
        // Else do everything else
        else
        {
            // Movement
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
    }

    private void FixedUpdate()
    {
    }

    // For detecting if the player is within a reasonable attacking range
    private void OnTriggerEnter(Collider other)
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

    public void TakeDmg(float dmg)
    {
        if (!dying)
        {
            health -= dmg;
            bossHealthBar.SetHealth(health, maxHealth);
            print("Health now: " + health);
        }
    }

    public void Die()
    {
        if (!dying)
        {
            dying = true;
            PlayerStats.Instance.xp += expAmt;
            EventManager.Instance.PlayerStatsUpdated();
            EventManager.Instance.runStats.enemiesKilled++;
            navMeshAgent.velocity = Vector3.zero;
            navMeshAgent.enabled = false;
            PersistentUpgradeManager.Instance.IncCurrency(1);
            portal.SetActive(true);
            StartCoroutine(DeathAnimation());
        }
    }

    // Movement
    // IEnums for crawl/rotate?

    // Damage Taken
    // TODO: Alter timings to match animation speeds
    private IEnumerator DamageLevel1()
    {
        animator.SetBool("Destroyed1", true);
        yield return new WaitForSeconds(3);
        animator.SetBool("Destroyed1", false);
    }

    private IEnumerator DamageLevel2()
    {
        animator.SetBool("Destroyed2", true);
        yield return new WaitForSeconds(3);
        animator.SetBool("Destroyed2", false);
    }

    private IEnumerator DamagedRoar()
    {
        animator.SetBool("DamagedRoar", true);
        yield return new WaitForSeconds(3);
        animator.SetBool("DamagedRoar", false);
    }

    // Death
    private IEnumerator DeathAnimation()
    {
        animator.SetBool("Dying", true);
        yield return new WaitForSeconds(3);
        animator.SetBool("Dying", false);
    }

    // Attacks
    private IEnumerator Roar()
    {
        // Don't set false here, instead set false in followup attacks
        animator.SetBool("Roaring", true);
        yield return new WaitForSeconds(3);
    }

    private IEnumerator TongueAttack()
    {
        animator.SetBool("TongueAttacking", true);
        yield return new WaitForSeconds(5);
        animator.SetBool("TongueAttacking", false);
        attacking = false;
    }

    private IEnumerator HornAttack()
    {
        animator.SetBool("HornAttacking", true);
        yield return new WaitForSeconds(3);
        animator.SetBool("HornAttacking", false);
        attacking = false;
    }

    private IEnumerator RamAttack()
    {
        // Setting roaring false here since we come from roaring and need it to be true to attack
        animator.SetBool("RamAttacking", true);
        yield return new WaitForSeconds(3);
        animator.SetBool("RamAttacking", false);
        animator.SetBool("Roaring", false);
        attacking = false;
    }

    private IEnumerator SlamAttack()
    {
        // Setting roaring false here since we come from roaring and need it to be true to attack
        animator.SetBool("SlamAttacking", true);
        yield return new WaitForSeconds(3);
        animator.SetBool("SlamAttacking", false);
        animator.SetBool("Roaring", false);
        attacking = false;
    }
}