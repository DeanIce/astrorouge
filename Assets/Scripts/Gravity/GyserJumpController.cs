using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyserJumpController : MonoBehaviour
{

    public GameObject player;
    Vector3 direction;
    public float jumpForce = 500f;

    // Update is called once per frame
    void Update()
    {
        // generate vector in the direction of jump pad's y axis multiplied with a factor jumpForce
        direction = jumpForce*transform.TransformDirection(transform.up);
    }

    private void OnCollisionEnter(Collision collision) {
        // MAKE THIS INTO A CUBE OR SOMETHING TO TRIGGER INSTEAD
        if (((1 << collision.gameObject.layer) | LayerMask.GetMask("Player")) == LayerMask.GetMask("Player")) // applies only to object "player"
        {
            player = collision.gameObject;
            // apply force to the Player's rigidbody to let him "jump"
            player.GetComponent<Rigidbody>().AddForce(direction, ForceMode.Impulse);
        }
    }
}
