using System.Collections;
using Gravity;
using Managers;
using UnityEngine;

public class BasicEnemyAgent : MonoBehaviour, IEnemy
{
    // Special note:
    // I have tailored my code from my 3541 class and used it here. This code has some 3rd party references
    // in it, but it is also being partially reused from a previous project.

    // Public variables that the game manager or other objects may need
    public float health;
    public float movementSpeed;
    [SerializeField] private GameObject detector;
    [SerializeField] private GameObject body;
    [SerializeField] private float attackRange;
    private readonly Color green = new(0, 1, 0, 0.5f);
    private readonly Vector3 jumpForce = new(0f, 20f, 0f);

    // Swapping to collider and layer based detection
    private readonly int playerLayer = 9;
    private readonly Color red = new(1, 0, 0, 0.5f);
    private Quaternion deltaRotation;
    protected float despawnTime = 10;
    private Renderer detectorRenderer;
    private float distanceToGround;
    private Vector3 eulerAngleVelocity;
    private bool hunting;

    private bool iAmAlive = true;

    private int leftOrRight;

    private int randomRotation;

    // Private enemy specific variables
    private bool attacking = false;
    public bool Attacking { get { return attacking; } set { attacking = value; } }
    private Rigidbody rb;
    private bool rotating;
    private Rigidbody targetRb;
    public float AttackRange => attackRange;
    public float Health => health;
    public bool Wandering { get; private set; }

    public GameObject Body => body;
    public bool Dying { get; set; }

    public virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        detectorRenderer = detector.GetComponent<Renderer>();
        Dying = false;
    }

    public virtual void FixedUpdate()
    {
        // Two states, either hunting or wandering
        if (!hunting && !Dying)
            Wander(body.transform.forward);
        else if (Dying) DoGravity();

        if (!hunting)
            detectorRenderer.material.SetColor("_BaseColor", green);
        else
            detectorRenderer.material.SetColor("_BaseColor", red);
    }

    private void OnDrawGizmos()
    {
        // Todo(Logan): draw gizmos
        // var size = detector.transform.localScale;
        // Gizmos.DrawMesh(detector.GetComponent<MeshFilter>().sharedMesh);
    }

    // Detected
    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == playerLayer)
        {
            Wandering = false;
            hunting = true;
        }
    }

    // Lost Player
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == playerLayer)
        {
            Wandering = true;
            hunting = false;
        }
    }

    // Hunting
    public virtual void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == playerLayer && !Dying) Hunt(other);
    }

    public virtual void Wander(Vector3 direction)
    {
        DoGravity();

        // This code is referenced from Unity documentation
        rb.MovePosition(rb.position + direction * Time.deltaTime * movementSpeed);
        if (!rotating)
            StartCoroutine(Rotate());
        else
        {
            deltaRotation = Quaternion.Euler(eulerAngleVelocity * Time.deltaTime);
            rb.MoveRotation(rb.rotation * deltaRotation);
        }
    }

    public virtual void Hunt(Collider target)
    {
        DoGravity();

        //attacking
        if (Physics.Raycast(transform.position, Body.transform.forward, AttackRange - 0.1f, playerLayer))
        {
            //it makes more sense of the !attacking condition to just be above but for some reason it doesn't work there
            if (!attacking && Health > 0) StartCoroutine(Attack());
        }
        else
        {
            if (!attacking)
            {
                // NEW MOVEMENT HERE
                targetRb = target.GetComponent<Rigidbody>();
                rb.MovePosition(rb.position + (target.transform.position - rb.position) * Time.deltaTime * movementSpeed);
                body.transform.RotateAround(transform.position, transform.up,
                    -Vector3.SignedAngle(target.transform.position - transform.position, body.transform.forward, transform.up) /
                    10);

                // Jumping - commented as only works in 2d but could bring back if desired?
                //if (targetRb.transform.position.y > transform.position.y && IsGrounded()) Jump();
            }
        }
    }

    public virtual IEnumerator Attack()
    {
        RaycastHit[] hits;
        //rend.enabled = true;
        attacking = true;
        yield return new WaitForSeconds(1f);
        hits = Physics.RaycastAll(transform.position, Body.transform.forward, AttackRange, playerLayer);
        if (hits.Length != 0)
        {
            //check for the player in the things the ray hit by whether it has a PlayerDefault
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.gameObject.GetComponent<PlayerDefault>() != null)
                {
                    hit.collider.gameObject.GetComponent<PlayerDefault>().TakeDmg(5);
                }
            }
        }
        //rend.enabled = false;
        attacking = false;
    }

    public void TakeDmg(float dmg)
    {
        if (!Dying)
        {
            // Temp, add damage negation and other maths here later.
            health -= dmg;
            gameObject.GetComponent<HealthBarUI>().SetHealth(health);
            // make damage popup TODO:: change the "false" to when this is a critical hit.
            // I think this would require adding a parameter and passing the
            // critical hit chance, or whenever the crit is defined.
            DamagePopupUI.Create(transform, transform.rotation, (int) dmg, false);

            if (health <= 0f && iAmAlive) Die();
        }
    }

    public virtual void Die()
    {
        iAmAlive = false;
        DropManager.Instance.SpawnItem(transform.position, transform.rotation);
        gameObject.GetComponent<HealthBarUI>().HideHealth();
        StartCoroutine(DestroyLater());
    }

    private IEnumerator DestroyLater()
    {
        Dying = true;
        yield return new WaitForSecondsRealtime(despawnTime);
        Destroy(gameObject);
    }

    public void DoGravity()
    {
        // Gravity
        var sumForce = GravityManager.GetGravity(transform.position, out var upAxis);
        rb.AddForce(sumForce * Time.deltaTime);
        Debug.DrawLine(transform.position, sumForce, Color.blue);

        // Upright?
        rb.MoveRotation(Quaternion.FromToRotation(transform.up, upAxis) * transform.rotation);
    }

    // Raycast jumping and grounded idea comes from here: https://answers.unity.com/questions/196381/how-do-i-check-if-my-rigidbody-player-is-grounded.html
    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, distanceToGround + 0.1f);
    }

    private void Jump()
    {
        rb.AddForce(jumpForce);
    }

    private IEnumerator Rotate()
    {
        // Convention
        // 1 = Left
        // 2 = Right
        // This is so they don't spaz back and forth and take big rotation strides
        // If we did it from -200 to 200, it would avg to values like 0 and 1
        rotating = true;
        leftOrRight = Random.Range(1, 3);

        if (leftOrRight == 1)
            randomRotation = Random.Range(-200, -100);
        else
            randomRotation = Random.Range(100, 200);

        eulerAngleVelocity = new Vector3(0, randomRotation, 0);
        // Note, increase this time to get slower turns and more "thinking"
        yield return new WaitForSeconds(0.5f);
        rotating = false;
    }

    // Temp, used for testing and debugging
    /*void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(rb.position, new Vector3(20, 5, 20));
        Gizmos.DrawWireCube(playerBounds.center, new Vector3(1, 2, 1));
    }*/
}