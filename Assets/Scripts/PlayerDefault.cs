using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDefault : MonoBehaviour, IPlayer
{
    Vector2 direction;
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {

    }

    private void OnDisable()
    {
        
    }

    public void Move(Vector2 direction)
    {
        this.direction = direction;
    }

    public void Jump()
    {
        throw new System.NotImplementedException();
    }
}
