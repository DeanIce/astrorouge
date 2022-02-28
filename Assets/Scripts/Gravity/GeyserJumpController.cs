using UnityEngine;

public class GeyserJumpController : MonoBehaviour
{
    [SerializeField] private float jumpForce = 500f;
    private LayerMask collisionLayer;

    private void Start()
    {
        collisionLayer = LayerMask.GetMask("Player"); // applies only to object "player"
    }

    private void OnCollisionEnter(Collision collision) {
        // MAKE THIS INTO A CUBE OR SOMETHING TO TRIGGER INSTEAD
        if (((1 << collision.gameObject.layer) | collisionLayer) == collisionLayer)
        {
            // apply force to the Player's rigidbody to let him "jump"
            collision.gameObject.GetComponent<Rigidbody>().AddForce(jumpForce * transform.up, ForceMode.Impulse);
        }
    }
}
