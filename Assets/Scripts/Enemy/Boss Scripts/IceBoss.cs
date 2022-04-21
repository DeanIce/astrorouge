using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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

    // Status stuff
    private bool dying;
    private bool attacking;
    private bool hunting;
    private bool inRange;
    public float health;
    public float movementSpeed;

    // Movement stuff
    private NavMeshAgent navMeshAgent;

    // view angles
    private float rollAngle = 1f;
    private float crawlForwardAngle = 10f;
    private float crawlLeftRightAngle = 20f;

    // damage stuff
    [SerializeField] private float attackRange;
    [SerializeField] private float attackRange2;
    [SerializeField] private int damage;
    [SerializeField] private int damage2;

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
        // DEBUGS
        // Debug.DrawLine(transform.position, transform.position + transform.forward * attackRange, Color.red, 1f);
        // Debug.DrawLine(transform.position, transform.position + transform.forward * attackRange2, Color.green, 1f);

        // if (InRollRange()) {
        //     StartCoroutine(RollAttack());
        // }

        if (InCrawlForwardRange()) {
            print("in crawl forward range");
            // StartCoroutine(RollAttack());
        }
        else if (InCrawlLeftRange()) {
            print("in crawl left range");
            //animator.SetBool("CrawlLeft_RM", true);
        }
        else if (InCrawlRightRange()) {
            print("in crawl right range");
            //animator.SetBool("CrawlLeft_RM", true);
        }

        // Attack();
        // if (!inRange)
        // {
        //     animator.SetBool("CrawlForward_RM", true);
        //     navMeshAgent.isStopped = false;
        //     navMeshAgent.destination = player.transform.position;
        // }
        // else
        // {
        //     navMeshAgent.isStopped = true;
        //     animator.SetBool("CrawlForward_RM", false);
        // }
    }

    void FixedUpdate()
    {
        
    }

    // FOV
    private bool InRollRange() {
        return Vector3.Angle(transform.forward, player.transform.position - transform.position) <= rollAngle;
    }

    private bool InCrawlForwardRange() {
        return Vector3.Angle(transform.forward, player.transform.position - transform.position) <= crawlForwardAngle;
    }

    private bool InCrawlLeftRange() {
        float angle = Vector3.Angle(transform.forward, player.transform.position - transform.position);
        return (angle <= crawlLeftRightAngle) && (AngleDir(transform.forward, player.transform.position, transform.up) < 0);
    }

    private bool InCrawlRightRange() {
        float angle = Vector3.Angle(transform.forward, player.transform.position - transform.position);
        return (angle <= crawlLeftRightAngle) && (AngleDir(transform.forward, player.transform.position, transform.up) > 0);
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

    private void OnTriggerStay(Collider other) {
        // Convention: Player layer is 9
        if (other.gameObject.layer == 9)
        {
            // increment timer, call attack when timer is done -> good for rolling
            // must reset timer after attack
            // reset timer in OnTriggerExit
            // Debug.Log("In Range!");
            inRange = true;
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

    private void Attack()
    {
        if (!attacking) {
            float randomAttack = Random.value;
            if (randomAttack < 0.25) {
                StartCoroutine(JumpAttack());
            }
            else if (randomAttack < 0.5) {
                StartCoroutine(ComboAttack());
            }
            else if (randomAttack < 0.75) {
                StartCoroutine(LeftAttack());
            }
            else {
                StartCoroutine(RightAttack());
            }
        }
    }

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
        animator.SetBool("Death", true);
        yield return new WaitForSeconds(2.333f);
        animator.SetBool("Death", false);
    }

    // Attacks
    IEnumerator JumpAttack()
    {
        attacking = true;
        animator.SetBool("JumpAttack_RM", true);
        yield return new WaitForSeconds(1.667f);
        animator.SetBool("JumpAttack_RM", false);
        attacking = false;
    }

    IEnumerator LeftAttack()
    {
        attacking = true;
        animator.SetBool("LeftAttack_RM", true);
        yield return new WaitForSeconds(1.333f);
        animator.SetBool("LeftAttack_RM", false);
        attacking = false;
    }

    IEnumerator RightAttack()
    {
        attacking = true;
        animator.SetBool("RightAttack_RM", true);
        yield return new WaitForSeconds(1.333f);
        animator.SetBool("RightAttack_RM", false);
        attacking = false;
    }

    IEnumerator ComboAttack()
    {
        attacking = true;
        animator.SetBool("ComboAttack_RM", true);
        yield return new WaitForSeconds(2.167f);
        animator.SetBool("ComboAttack_RM", false);
        attacking = false;
    }

    IEnumerator RollAttack()
    {
        attacking = true;
        animator.SetBool("Rolling", true);
        yield return new WaitForSeconds(1.333f);
        animator.SetBool("Rolling", false);
        attacking = false;
    }

    // DAMAGE
    public void DoDamage()
    {
        RaycastHit[] hits;

        hits = Physics.RaycastAll(transform.position, transform.forward, attackRange, LayerMask.GetMask("Player"));
        if (hits.Length != 0)
        {
            //check for the player in the things the ray hit by whether it has a PlayerDefault
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.gameObject.GetComponent<PlayerDefault>() != null)
                {
                    hit.collider.gameObject.GetComponent<PlayerDefault>().TakeDmg(damage);
                }
            }
        }
    }

    public void DoDamage2()
    {
        RaycastHit[] hits;

        hits = Physics.RaycastAll(transform.position, transform.forward, attackRange2, LayerMask.GetMask("Player"));
        if (hits.Length != 0)
        {
            //check for the player in the things the ray hit by whether it has a PlayerDefault
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.gameObject.GetComponent<PlayerDefault>() != null)
                {
                    hit.collider.gameObject.GetComponent<PlayerDefault>().TakeDmg(damage2);
                }
            }
        }
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
