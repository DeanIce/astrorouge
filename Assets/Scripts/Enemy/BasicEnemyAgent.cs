using System.Collections;
using System.Collections.Generic;
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

    // Private enemy specific variables
    Rigidbody rb;
    Quaternion deltaRotation;
    Vector3 eulerAngleVelocity;
    Bounds b;
    Bounds playerBounds;
    Vector3 newDirection;
    Vector3 playerPosition;
    int randomRotation;
    int leftOrRight;
    private bool wandering = false;
    private bool hunting = false;
    private bool rotating = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        b = new Bounds(rb.position, new Vector3(20, 5, 20));
        playerBounds = playerCollider.bounds;
    }

    void Update()
    {
        // Maybe just set b center pos instead
        // Potential Bug here **
        b = new Bounds(rb.position, new Vector3(20, 5, 20));
        playerBounds = playerCollider.bounds;
    }

    void FixedUpdate()
    {
        // Two states, either hunting or wandering
        if (!hunting && wandering)
        {
            wander(transform.forward);
        } 

        if (b.Intersects(playerBounds))
        {
            wandering = false;
            hunting = true;
            hunt();
        } 
        else
        {
            wandering = true;
            hunting = false;
        }
    }

    public void wander(Vector3 direction)
    {
        // This code is referenced from Unity documentation
        rb.MovePosition(rb.position + direction * Time.deltaTime * movementSpeed);
        if (!rotating)
        {
            StartCoroutine(rotate());
        }
        else
        {
            deltaRotation = Quaternion.Euler(eulerAngleVelocity * Time.deltaTime);
            rb.MoveRotation(rb.rotation * deltaRotation);
        }
    }

    public void hunt()
    {
        // NOTE: May need to add offset to playerBounds center, potential bug here ***
        // Rotation referenced from unity documentation
        playerPosition = new Vector3(playerBounds.center.x, transform.position.y, playerBounds.center.z);
        Debug.Log("Hunting towards " + playerBounds.center.x + " " + playerBounds.center.z);

        // Actual movement and rotation
        transform.position = Vector3.MoveTowards(transform.position, playerPosition, movementSpeed * Time.deltaTime);
        newDirection = Vector3.RotateTowards(transform.forward, playerPosition - transform.position, movementSpeed * Time.deltaTime, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDirection, transform.up);

        // Detect if player is above enemy, if so, then we want to jump
        if (playerBounds.center.y > transform.position.y)
        {
            Debug.Log("Jump!");
        }
    }

    public void takeDmg(float dmg)
    {
        // Temp, add damage negation and other maths here later.
        health -= dmg;
        if (health < 0f)
        {
            die();
        }
    }
    public void die()
    {
        // Temp, add animation and call other methods here later.
        GameObject.Destroy(this);
    }

    IEnumerator rotate()
    {
        // Convention
        // 1 = Left
        // 2 = Right
        // This is so they don't spaz back and forth and take big rotation strides
        // If we did it from -200 to 200, it would avg to values like 0 and 1
        rotating = true;
        leftOrRight = Random.Range(1, 3);

        if (leftOrRight == 1)
        {
            randomRotation = Random.Range(-200, -100);
        }
        else
        {
            randomRotation = Random.Range(100, 200);
        }

        eulerAngleVelocity = new Vector3(0, randomRotation, 0);
        // Note, increase this time to get slower turns and more "thinking"
        yield return new WaitForSeconds(0.5f);
        rotating = false;

    }

    // Temp, used for testing and debugging
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(rb.position, new Vector3(20, 5, 20));
        Gizmos.DrawWireCube(playerBounds.center, new Vector3(1, 2, 1));
    }
}
