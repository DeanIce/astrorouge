using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : BasicEnemyAgent
{
    private float timeToLive = 5f;
    private Rigidbody r123;
    
    // Start is called before the first frame update
    public override void Start()
    {
        r123 = GetComponent<Rigidbody>();
        base.Start();
    }

    // Update is called once per frame
    public override void FixedUpdate()
    {
        timeToLive -= Time.deltaTime;
        if (timeToLive <= 0) Destroy(gameObject);
        base.FixedUpdate();
    }

    public override void Wander(Vector3 direction)
    {
        // This code is referenced from Unity documentation
        r123.MovePosition(r123.position + direction * Time.deltaTime * movementSpeed);
    }

    public override void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.GetMask("Player"))
        {
            collision.gameObject.GetComponent<PlayerDefault>().TakeDmg(5);
            Destroy(gameObject);
        }
    }
}
