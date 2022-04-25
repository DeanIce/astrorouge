using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeDamage : MonoBehaviour
{
    // private variables
    private float spikeTimer = 5f;
    private float moveSpeed = 10f;

    // public variables
    public int damage = 2;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y - 10, transform.position.z);
    }

    void Update() {
        spikeTimer -= Time.deltaTime;
        // if (spikeTimer < 0) {
        //     Destroy(gameObject);
        // } else 
        if (transform.position.y < 0) {
            transform.position += transform.up * moveSpeed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
        {
            // print("claw collider");
            other.transform.root.gameObject.GetComponent<PlayerDefault>()?.TakeDmg(damage);
        }
}
