using System.Collections;
using Managers;
using UI;
using UnityEngine;
using UnityEngine.AI;

public class IceBoss : MonoBehaviour
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
    public GameObject portal;

    public int xpGift = 150;

    // Status stuff
    public float movementSpeed;

    public string movementState = "forward";

    // damage stuff
    [SerializeField] private float attackRange;
    [SerializeField] private float attackRange2;
    public int damage = 20;
    public int damage2 = 30;
    public int attackDamage;

    public CapsuleCollider LeftClaw;
    public CapsuleCollider RightClaw;
    public BoxCollider RollCollider;
    public CapsuleCollider HeadCollider;

    public GameObject spikes1;
    public GameObject spikes2;
    private readonly float crawlForwardAngle = 10f;
    private readonly float crawlLeftRightAngle = 20f;

    // view angles
    private readonly float rollAngle = 5f;
    private readonly float rollTimerStart = 3f;

    // Animation stuff
    private Animator animator;
    private bool attacking;

    // Status stuff
    private bool dying;
    private float health;
    private bool hunting;
    private bool inRange;

    // Movement stuff
    private NavMeshAgent navMeshAgent;

    // Components
    private Rigidbody rb;
    private float rollTimer;

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
        animator = GetComponent<Animator>();
        portal.SetActive(false);
        dying = false;
        inRange = false;
        attacking = false;
        rollTimer = rollTimerStart;

        // claw colliders
        LeftClaw.isTrigger = false;
        RightClaw.isTrigger = false;
        RollCollider.isTrigger = false;
    }

    private void Update()
    {
        bossHealthBar.SetHealth(health, maxHealth);

        if (health < 0)
            Die();
        else
        {
            // DEBUG
            // print($"movement state: {movementState}");

            if (animator.GetBool("Rolling") == false)
            {
                if (InCrawlBackwardRange())
                {
                    if (movementState != "backward")
                    {
                        StopCrawl();
                        StartBackwardCrawl();
                    }
                }
                else if (InCrawlForwardRange())
                {
                    //print("in crawl forward range");
                    if (movementState != "forward")
                    {
                        StopCrawl();
                        StartForwardCrawl();
                    }

                    if (InRollRange()) RollAttack();
                    if (InAttackRange()) Attack();
                }
                else if (InCrawlLeftRange())
                {
                    if (movementState != "left")
                    {
                        StopCrawl();
                        StartLeftCrawl();
                    }
                }
                else if (InCrawlRightRange())
                {
                    //print("in crawl right range");
                    if (movementState != "right")
                    {
                        StopCrawl();
                        StartRightCrawl();
                    }
                }
                else if (!inRange)
                {
                    animator.SetBool("CrawlForward_RM", true);
                    navMeshAgent.isStopped = false;
                    navMeshAgent.destination = player.transform.position;
                }
                else
                {
                    navMeshAgent.isStopped = true;
                    animator.SetBool("CrawlForward_RM", false);
                }
            }
            else
            {
                rollTimer -= Time.deltaTime;
                if (rollTimer < 0) {
                    StopRolling();
                }
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
            // Debug.Log("In Range!");
            inRange = true;
        }
    }

    // For detecting if the player leaves the reasonable attacking range
    private void OnTriggerExit(Collider other)
    {
        // Convention: Player layer is 9
        if (other.gameObject.layer == 9)
        {
            // Debug.Log("Left Range!");
            inRange = false;
        }
    }

    // death
    public void Die()
    {
        if (!dying)
        {
            dying = true;
            // Give XP for killing the enemy
            PlayerStats.Instance.xp += xpGift;
            EventManager.Instance.PlayerStatsUpdated();
            EventManager.Instance.runStats.enemiesKilled++;

            PersistentUpgradeManager.Instance.IncCurrency(1);

            navMeshAgent.velocity = Vector3.zero;
            navMeshAgent.enabled = false;
            portal.SetActive(true);

            StartCoroutine(DeathAnimation());
        }
    }

    // Death
    private IEnumerator DeathAnimation()
    {
        HeadCollider.enabled = false;
        LeftClaw.enabled = false;
        RightClaw.enabled = false;

        animator.SetBool("Death", true);
        yield return new WaitForSeconds(2.333f);
        animator.SetBool("Dead", true);
    }

    // FOV
    private bool InRollRange()
    {
        float angle = Vector3.Angle(transform.forward, player.transform.position - transform.position);
        float distance = Vector3.Distance(transform.position, player.transform.position);
        return angle <= rollAngle;
    }

    private bool InCrawlForwardRange()
    {
        float angle = Vector3.Angle(transform.forward, player.transform.position - transform.position);
        float distance = Vector3.Distance(transform.position, player.transform.position);
        return angle <= crawlForwardAngle && distance >= 15;
    }

    private bool InCrawlBackwardRange()
    {
        float angle = Vector3.Angle(transform.forward, player.transform.position - transform.position);
        float distance = Vector3.Distance(transform.position, player.transform.position);
        return distance < 0 || angle <= crawlForwardAngle && distance < 15;
    }

    private bool InCrawlLeftRange()
    {
        float angle = Vector3.Angle(transform.forward, player.transform.position - transform.position);
        return angle <= crawlLeftRightAngle && AngleDir(transform.forward, player.transform.position, transform.up) < 0;
    }

    private bool InCrawlRightRange()
    {
        float angle = Vector3.Angle(transform.forward, player.transform.position - transform.position);
        return angle <= crawlLeftRightAngle && AngleDir(transform.forward, player.transform.position, transform.up) > 0;
    }

    private bool InAttackRange()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);
        return distance >= 15 && distance <= 20;
    }

    private bool InJumpAttackRange()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);
        return distance <= 17 && distance >= 13;
    }

    private bool InClawAttackRange()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);
        return distance <= 22 && distance >= 18;
    }

    public void TakeDmg(float dmg)
    {
        if (!dying)
        {
            health -= dmg;
            bossHealthBar.SetHealth(health, maxHealth);
            Flinch();
        }
    }

    private void Flinch()
    {
        float flinchChance = Random.value;
        if (flinchChance <= 0.1) GetHitFront();
    }

    // movement states
    private void StartLeftCrawl()
    {
        movementState = "left";
        animator.SetBool("CrawlLeft_RM", true);
    }

    private void StartRightCrawl()
    {
        movementState = "right";
        animator.SetBool("CrawlRight_RM", true);
    }

    private void StartForwardCrawl()
    {
        movementState = "forward";
        animator.SetBool("CrawlForward_RM", true);
    }

    private void StartBackwardCrawl()
    {
        movementState = "backward";
        animator.SetBool("CrawlBackward_RM", true);
    }

    private void StopCrawl()
    {
        switch (movementState)
        {
            case "forward":
                animator.SetBool("CrawlForward_RM", false);
                return;
            case "left":
                animator.SetBool("CrawlLeft_RM", false);
                return;
            case "right":
                animator.SetBool("CrawlRight_RM", false);
                return;
            case "backward":
                animator.SetBool("CrawlBackward_RM", false);
                return;
        }
    }

    // normal attacking
    private void Attack()
    {
        if (!attacking)
        {
            float randomAttack = Random.value;
            if (randomAttack < 0.25 && InJumpAttackRange())
                StartCoroutine(JumpAttack());
            else if (randomAttack < 0.5 && InClawAttackRange())
                StartCoroutine(ComboAttack());
            else if (randomAttack < 0.75 && InClawAttackRange())
                StartCoroutine(LeftAttack());
            else if (InClawAttackRange()) StartCoroutine(RightAttack());
        }
    }

    // Damage Taken
    private IEnumerator GetHitRight()
    {
        animator.SetBool("HitRight", true);
        yield return new WaitForSeconds(1);
        animator.SetBool("HitRight", false);
    }

    private IEnumerator GetHitLeft()
    {
        animator.SetBool("HitLeft", true);
        yield return new WaitForSeconds(1);
        animator.SetBool("HitLeft", false);
    }

    private IEnumerator GetHitFront()
    {
        animator.SetBool("HitFront", true);
        yield return new WaitForSeconds(1);
        animator.SetBool("HitFront", false);
    }

    private IEnumerator GetHitBack()
    {
        animator.SetBool("HitBack", true);
        yield return new WaitForSeconds(1);
        animator.SetBool("HitBack", false);
    }


    // Attacks
    private IEnumerator JumpAttack()
    {
        attacking = true;
        attackDamage = damage2;
        animator.SetBool("JumpAttack_RM", true);
        yield return new WaitForSeconds(1.667f);
        animator.SetBool("JumpAttack_RM", false);
        attacking = false;
    }

    private IEnumerator LeftAttack()
    {
        attacking = true;
        attackDamage = damage;
        animator.SetBool("LeftAttack_RM", true);
        yield return new WaitForSeconds(1.333f);
        animator.SetBool("LeftAttack_RM", false);
        attacking = false;
    }

    private IEnumerator RightAttack()
    {
        attacking = true;
        attackDamage = damage;
        animator.SetBool("RightAttack_RM", true);
        yield return new WaitForSeconds(1.333f);
        animator.SetBool("RightAttack_RM", false);
        attacking = false;
    }

    private IEnumerator ComboAttack()
    {
        attacking = true;
        attackDamage = damage;
        animator.SetBool("ComboAttack_RM", true);
        yield return new WaitForSeconds(2.167f);
        animator.SetBool("ComboAttack_RM", false);
        attacking = false;
    }

    private void RollAttack()
    {
        attackDamage = damage2;
        RollCollider.isTrigger = true;
        animator.SetBool("Rolling", true);

        InvokeRepeating("SpawnSpikes", 0.1f, 0.5f);
    }

    public void StopRolling()
    {
        rollTimer = rollTimerStart;
        CancelInvoke();
        animator.SetBool("Rolling", false);
        RollCollider.isTrigger = false;
    }

    private void SpawnSpikes() {
        if (Random.value < 0.5) {
            GameObject instance = Instantiate(spikes1, transform.position - 2 * transform.forward + (Random.value) * 3 * transform.right, transform.rotation);
        } else {
            GameObject instance = Instantiate(spikes2, transform.position- 2 * transform.forward - (Random.value) * 3 * transform.right, transform.rotation);
        }
    }

    // Attack Colliders
    public void TurnClawColliderOn()
    {
        LeftClaw.isTrigger = true;
        RightClaw.isTrigger = true;
    }

    public void TurnClawColliderOff()
    {
        LeftClaw.isTrigger = false;
        RightClaw.isTrigger = false;
    }

    //returns -1 when to the left, 1 to the right, and 0 for forward/backward
    public float AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up)
    {
        Vector3 perp = Vector3.Cross(fwd, targetDir);
        float dir = Vector3.Dot(perp, up);

        if (dir > 0.0f)
            return 1.0f;
        if (dir < 0.0f)
            return -1.0f;
        return 0.0f;
    }
}