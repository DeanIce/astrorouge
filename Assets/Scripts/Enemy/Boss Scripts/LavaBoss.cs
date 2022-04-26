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
    private bool flinching;

    // Attack Colliders
    public DamageOnenter tongueDamageScript;
    public DamageOnenter ramDamageScript;
    public DamageOnenter slamDamageScript;

    // Attack Damage
    private float damageToDo;
    public float slamDamage;
    public float tongueDamage;
    public float ramDamage;

    // Attack fx
    public GameObject slamEffect;
    public GameObject fireball;

    // Misc
    public int expAmt;
    private float distance;

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
        distance = Vector3.Distance(transform.position, player.transform.position);

        // May need to be GetComponentInChildren
        portal.SetActive(false);
        animator = GetComponent<Animator>();
        dying = false;
        inRange = false;
        attacking = false;

    }

    private void Update()
    {
        // To prevent endless walking on top of
        distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance <= 25)
        {
            inRange = true;
        }

        // Flinching
        if (health < health / 50)
        {
            // await
        }

        // Die?
        if (health < 0)
        {
            Die();
        }
        // Else do everything else
        else
        {
            // Movement
            if (!inRange && !attacking)
            {
                animator.SetBool("Crawling", true);
                navMeshAgent.isStopped = false;
                navMeshAgent.destination = player.transform.position;
            }
            else if (InAttackRange())
            {
                navMeshAgent.isStopped = true;
                animator.SetBool("Crawling", false);
                Attack();
            }
            else if (inRange && !attacking)
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

    public void Attack()
    {
        if (!attacking)
        {
            attacking = true;
            float randomAttack = Random.value;
            if (randomAttack < 0.25 && distance < 35)
            {
                print("Roar then slam");
                StartCoroutine(Roar(1));
            }
            else if (randomAttack >= 0.25 && randomAttack < 0.5 && distance < 60)
            {
                print("tongue");
                StartCoroutine(TongueAttack());
            }
            else if (randomAttack >= 0.5 && randomAttack < 0.85 && distance < 25)
            {
                print("Roar then ram");
                StartCoroutine(Roar(2));
            }
            else
            {
                print("Horn");
                StartCoroutine(HornAttack());
            }
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
            PersistentUpgradeManager.Instance.IncCurrency(1); // Assuming we are first boss
            portal.SetActive(true);
            StartCoroutine(DeathAnimation());
        }
    }

    private bool InAttackRange()
    {
        // TODO: Edit numbers
        distance = Vector3.Distance(transform.position, player.transform.position);
        //print("Distance from player: " + distance);
        return distance >= 10 && distance <= 70;
    }

    public GameObject SpawnSlamEffects()
    {
        Vector3 newPos = transform.position + new Vector3(0f, 0f, 0f);
        GameObject slamObject = Instantiate(slamEffect, newPos, transform.rotation);
        return slamObject;
    }

    public void DestroySlamObject(GameObject slam)
    {
        Destroy(slam);
    }

    public void SpawnFireball()
    {
        Vector3 newPos = transform.position + new Vector3(5f, 12f, 0f);
        var temp = ProjectileFactory.Instance.CreateBasicProjectile(newPos, Vector3.Normalize(player.transform.position - newPos) * 50f, 
                   LayerMask.GetMask("Player"), 10, 15f);
        ProjectileFactory.Instance.SetSkin(temp, fireball);
        temp.transform.localScale *= 3f;
    }

    public void EnableTongueHit()
    {
        tongueDamageScript.EnableDoShit();
    }
    public void DisableTongueHit()
    {
        tongueDamageScript.DisableDoShit();
    }

    public void EnableRamHit()
    {
        ramDamageScript.EnableDoShit();
    }
    public void DisableRamHit()
    {
        ramDamageScript.DisableDoShit();
    }
    public void EnableSlamHit()
    {
        slamDamageScript.EnableDoShit();
    }
    public void DisableSlamHit()
    {
        slamDamageScript.DisableDoShit();
    }

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
    private IEnumerator Roar(int selection)
    {
        // Don't set false here, instead set false in followup attacks
        animator.SetBool("Roaring", true);
        yield return new WaitForSeconds(5.29f);
        if (selection == 1)
        {
            StartCoroutine(SlamAttack());
        }
        else if (selection == 2)
        {
            StartCoroutine(RamAttack());
        }
    }

    private IEnumerator TongueAttack()
    {
        damageToDo = tongueDamage;
        animator.SetBool("TongueAttacking", true);
        yield return new WaitForSeconds(2f);
        animator.SetBool("TongueAttacking", false);
        attacking = false;
    }

    private IEnumerator HornAttack()
    {
        // TODO: Spawn fireball
        animator.SetBool("HornAttacking", true);
        yield return new WaitForSeconds(0.50f);
        SpawnFireball();
        yield return new WaitForSeconds(1.59f);
        animator.SetBool("HornAttacking", false);
        attacking = false;
    }

    private IEnumerator RamAttack()
    {
        // Setting roaring false here since we come from roaring and need it to be true to attack
        damageToDo = ramDamage;
        animator.SetBool("RamAttacking", true);
        yield return new WaitForSeconds(3.05f);
        animator.SetBool("RamAttacking", false);
        animator.SetBool("Roaring", false);
        attacking = false;
    }

    private IEnumerator SlamAttack()
    {
        // Setting roaring false here since we come from roaring and need it to be true to attack
        damageToDo = slamDamage;
        animator.SetBool("SlamAttacking", true);
        yield return new WaitForSeconds(2.09f);
        GameObject slam = SpawnSlamEffects();
        yield return new WaitForSeconds(2.00f);
        DestroySlamObject(slam);
        animator.SetBool("SlamAttacking", false);
        animator.SetBool("Roaring", false);
        attacking = false;
    }
}