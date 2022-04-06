using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaBoss : MonoBehaviour
{
    // Animation stuff
    private Animator animator;

    // Components
    private Rigidbody rb;

    // Status stuff
    private bool dying;
    private float health;
    private float movementSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // May need to be GetComponentInChildren
        animator = GetComponent<Animator>();
        dying = false;
    }

    public void Die()
    {

    }
}
