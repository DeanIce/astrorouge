using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Managers;

public class IceBoss : MonoBehaviour
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
    public GameObject portal;

    public int xpGift = 150;

    // Status stuff
    private bool dying;
    private bool attacking;
    private float rollTimerStart = 3f;
    private float rollTimer;
    private bool hunting;
    private bool inRange;
    public float health;
    public float movementSpeed;

    public string movementState = "forward";

    // Movement stuff
    private NavMeshAgent navMeshAgent;

    // view angles
    private float rollAngle = 5f;
    private float crawlForwardAngle = 10f;
    private float crawlLeftRightAngle = 20f;

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

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
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

    void Update()
    {
        if (health < 0) {
            Die();
        } else {
            // DEBUG
            // print($"movement state: {movementState}");

            if (animator.GetBool("Rolling") == false) {
                if (InCrawlBackwardRange()) {
                    if (movementState != "backward") {
                        StopCrawl();
                        StartBackwardCrawl();
                    }
                }
                else if (InCrawlForwardRange()) {
                    //print("in crawl forward range");
                    if (movementState != "forward") {
                        StopCrawl();
                        StartForwardCrawl();
                    }
                    if (InRollRange()) {
                        RollAttack();
                    }
                    if (InAttackRange()) {
                        Attack();
                    }
                }
                else if (InCrawlLeftRange()) {
                    if (movementState != "left") {
                        StopCrawl();
                        StartLeftCrawl();
                    }
                }
                else if (InCrawlRightRange()) {
                    //print("in crawl right range");
                    if (movementState != "right") {
                        StopCrawl();
                        StartRightCrawl();
                    }
                } else if (!inRange)
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
            } else {
                rollTimer -= Time.deltaTime;
                if (rollTimer < 0) {
                    StopRolling();
                }
            }
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
            navMeshAgent.velocity = Vector3.zero;
            navMeshAgent.enabled = false;
            portal.SetActive(true);
            
            StartCoroutine(DeathAnimation());
        }
    }

    // Death
    IEnumerator DeathAnimation()
    {
        HeadCollider.enabled = false;
        LeftClaw.enabled = false;
        RightClaw.enabled = false;

        animator.SetBool("Death", true);
        yield return new WaitForSeconds(2.333f);
        animator.SetBool("Dead", true);
    }

    // FOV
    private bool InRollRange() {
        float angle = Vector3.Angle(transform.forward, player.transform.position - transform.position);
        float distance = Vector3.Distance(transform.position, player.transform.position);
        return (angle <= rollAngle);
    }

    private bool InCrawlForwardRange() {
        float angle = Vector3.Angle(transform.forward, player.transform.position - transform.position);
        float distance = Vector3.Distance(transform.position, player.transform.position);
        return (angle <= crawlForwardAngle) && (distance >= 15);
    }

    private bool InCrawlBackwardRange() {
        float angle = Vector3.Angle(transform.forward, player.transform.position - transform.position);
        float distance = Vector3.Distance(transform.position, player.transform.position);
        return (distance < 0) || (angle <= crawlForwardAngle) && (distance < 15);
    }

    private bool InCrawlLeftRange() {
        float angle = Vector3.Angle(transform.forward, player.transform.position - transform.position);
        return (angle <= crawlLeftRightAngle) && (AngleDir(transform.forward, player.transform.position, transform.up) < 0);
    }

    private bool InCrawlRightRange() {
        float angle = Vector3.Angle(transform.forward, player.transform.position - transform.position);
        return (angle <= crawlLeftRightAngle) && (AngleDir(transform.forward, player.transform.position, transform.up) > 0);
    }

    private bool InAttackRange() {
        float distance = Vector3.Distance(transform.position, player.transform.position);
        return distance >= 15 && distance <= 20;
    }
    private bool InJumpAttackRange() {
        float distance = Vector3.Distance(transform.position, player.transform.position);
        return distance <= 17 && distance >= 13;
    }

    private bool InClawAttackRange() {
        float distance = Vector3.Distance(transform.position, player.transform.position);
        return distance <= 22 && distance >= 18;
    }

    // For detecting if the player is within a reasonable attacking range
    void OnTriggerEnter(Collider other)
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

    public void TakeDmg(float dmg) {
        if (!dying) {
            health -= dmg;
            Flinch();
        }
    }

    private void Flinch() {
        float flinchChance = Random.value;
        if (flinchChance <= 0.1) {
            GetHitFront();
        }
    }

    // movement states
    private void StartLeftCrawl() {
        movementState = "left";
        animator.SetBool("CrawlLeft_RM", true);
    }

    private void StartRightCrawl() {
        movementState = "right";
        animator.SetBool("CrawlRight_RM", true);
    }

    private void StartForwardCrawl() {
        movementState = "forward";
        animator.SetBool("CrawlForward_RM", true);
    }

    private void StartBackwardCrawl() {
        movementState = "backward";
        animator.SetBool("CrawlBackward_RM", true);
    }

    private void StopCrawl() {
        switch (movementState) {
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

    public void StopRolling() {
        rollTimer = rollTimerStart;
        animator.SetBool("Rolling", false);
        RollCollider.isTrigger = false;
    }

    // normal attacking
    private void Attack()
    {
        if (!attacking) {
            float randomAttack = Random.value;
            if (randomAttack < 0.25 && InJumpAttackRange()) {
                StartCoroutine(JumpAttack());
            }
            else if (randomAttack < 0.5 && InClawAttackRange()) {
                StartCoroutine(ComboAttack());
            }
            else if (randomAttack < 0.75 && InClawAttackRange()) {
                StartCoroutine(LeftAttack());
            }
            else if (InClawAttackRange()) {
                StartCoroutine(RightAttack());
            }
        }
    }

    // Damage Taken
    IEnumerator GetHitRight()
    {
        animator.SetBool("HitRight", true);
        yield return new WaitForSeconds(1);
        animator.SetBool("HitRight", false);
    }

    IEnumerator GetHitLeft()
    {
        animator.SetBool("HitLeft", true);
        yield return new WaitForSeconds(1);
        animator.SetBool("HitLeft", false);
    }

    IEnumerator GetHitFront()
    {
        animator.SetBool("HitFront", true);
        yield return new WaitForSeconds(1);
        animator.SetBool("HitFront", false);
    }

    IEnumerator GetHitBack()
    {
        animator.SetBool("HitBack", true);
        yield return new WaitForSeconds(1);
        animator.SetBool("HitBack", false);
    }

    // Attacks
    IEnumerator JumpAttack()
    {
        attacking = true;
        attackDamage = damage2;
        animator.SetBool("JumpAttack_RM", true);
        yield return new WaitForSeconds(1.667f);
        animator.SetBool("JumpAttack_RM", false);
        attacking = false;
    }

    IEnumerator LeftAttack()
    {
        attacking = true;
        attackDamage = damage;
        animator.SetBool("LeftAttack_RM", true);
        yield return new WaitForSeconds(1.333f);
        animator.SetBool("LeftAttack_RM", false);
        attacking = false;
    }

    IEnumerator RightAttack()
    {
        attacking = true;
        attackDamage = damage;
        animator.SetBool("RightAttack_RM", true);
        yield return new WaitForSeconds(1.333f);
        animator.SetBool("RightAttack_RM", false);
        attacking = false;
    }

    IEnumerator ComboAttack()
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
    }

    // Attack Colliders
    public void TurnClawColliderOn() {
        LeftClaw.isTrigger = true;
        RightClaw.isTrigger = true;
    }

    public void TurnClawColliderOff() {
        LeftClaw.isTrigger = false;
        RightClaw.isTrigger = false;
    }

    //returns -1 when to the left, 1 to the right, and 0 for forward/backward
    public float AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up)
    {
        Vector3 perp = Vector3.Cross(fwd, targetDir);
        float dir = Vector3.Dot(perp, up);
 
        if (dir > 0.0f) {
            return 1.0f;
        } else if (dir < 0.0f) {
            return -1.0f;
        } else {
            return 0.0f;
        }
    }  
    
}
