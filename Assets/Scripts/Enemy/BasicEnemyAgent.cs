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
    public Collider playerCollider;
    [SerializeField] private GameObject detector;
    [SerializeField] private GameObject body;
    private readonly Color green = new(0, 1, 0, 0.5f);
    private readonly Vector3 jumpForce = new(0f, 20f, 0f);

    // Swapping to collider and layer based detection
    private readonly int playerLayer = 9;
    private readonly Color red = new(1, 0, 0, 0.5f);
    private Quaternion deltaRotation;
    private Renderer detectorRenderer;
    private float distanceToGround;
    private Vector3 eulerAngleVelocity;
    private bool hunting;
    private bool isGroundedVar = true;

    private int leftOrRight;

    //Bounds b;
    //Bounds playerBounds;
    private Vector3 newDirection;
    private Vector3 playerPosition;
    private int randomRotation;

    // Private enemy specific variables
    private Rigidbody rb;
    private bool rotating;
    private Rigidbody targetRb;
    public float Health => health;
    public bool Wandering { get; private set; }

    public GameObject Body => body;
    public bool Dying { get; set; }

    public virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        detectorRenderer = detector.GetComponent<Renderer>();
        Dying = false;
        // b = new Bounds(rb.position, new Vector3(20, 5, 20));
        // distanceToGround = b.extents.y;
        //playerBounds = playerCollider.bounds;
    }

    private void Update()
    {
        // OLD MOVEMENT HERE
        // Maybe just set b center pos instead
        // Potential Bug here **
        //b = new Bounds(rb.position, new Vector3(20, 5, 20));
        //playerBounds = playerCollider.bounds;

        // TEMP FOR TESTING DROPS
        /*if (Input.GetKeyDown(KeyCode.L))
        {
            Die();
        }*/
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

    // Swapping to collider based detection
    // This is for attacking
    public virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == playerLayer)
        {
            // Attack
        }
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
        {
            StartCoroutine(Rotate());
        }
        else
        {
            deltaRotation = Quaternion.Euler(eulerAngleVelocity * Time.deltaTime);
            rb.MoveRotation(rb.rotation * deltaRotation);
        }
    }

    public virtual void Hunt(Collider target)
    {
        DoGravity();

        // NEW MOVEMENT HERE
        targetRb = target.GetComponent<Rigidbody>();
        rb.MovePosition(rb.position + (target.transform.position - rb.position) * Time.deltaTime * movementSpeed);
        body.transform.RotateAround(transform.position, transform.up,
            -Vector3.SignedAngle(target.transform.position - transform.position, body.transform.forward, transform.up) /
            10);

        // Jumping
        if (targetRb.transform.position.y > transform.position.y && IsGrounded()) Jump();

        // OLD MOVEMENT HERE
        // NOTE: May need to add offset to playerBounds center, potential bug here ***
        // Rotation referenced from unity documentation
        /*playerPosition = new Vector3(playerBounds.center.x, transform.position.y, playerBounds.center.z);
        Debug.Log("Hunting towards " + playerBounds.center.x + " " + playerBounds.center.z);
        */
        // Actual movement and rotation
        /* transform.position = Vector3.MoveTowards(transform.position, playerPosition, movementSpeed * Time.deltaTime);
        newDirection = Vector3.RotateTowards(transform.forward, playerPosition - transform.position, movementSpeed * Time.deltaTime, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDirection, transform.up);*/

        // Detect if player is above enemy, if so, then we want to jump
        /* if (playerBounds.center.y > transform.position.y && IsGrounded())
        {
            Debug.Log("Jump!");
            // Jump();
        }*/
    }

    public void TakeDmg(float dmg)
    {
        if (!Dying)  {
            // Temp, add damage negation and other maths here later.
            health -= dmg;
            gameObject.GetComponent<HealthBarUI>().SetHealth(health);
            // make damage popup
            bool isCriticalHit = Random.Range(0, 100) < 30;
            DamagePopupUI.Create(transform.position + Vector3.up, (int) dmg, isCriticalHit);

            if (health <= 0f) Die();
        }
    }

    public virtual void Die()
    {
        // Temp, add animation and call other methods here later.
        DropManager.SpawnItem(transform.position, transform.rotation);
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