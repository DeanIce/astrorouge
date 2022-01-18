using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDefault : MonoBehaviour, IPlayer
{
    private PlayerInputActions playerInputActions;
    private InputAction movement;
    private Rigidbody rb;

    // Player stats
    private float speed = 6f;
    private float jumpSpeed = 15f;
    private float gravity = 45f;
    private int maxExtraJumps = 2; // Total jumps = maxExtraJumps + 1

    // Dynamic player info
    public float verticalVelocity;
    private bool isGrounded;
    private int extraJumpsLeft;

    // Constants
    private Transform groundCheck;
    private LayerMask groundMask;
    private const float groundDistance = 0.1f;
    private const float defaultVelocity = -1f;
    private const float terminalVelocity = -50f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        groundCheck = transform.Find("GroundCheck");
        groundMask = LayerMask.GetMask("Ground");
        verticalVelocity = defaultVelocity;
        extraJumpsLeft = maxExtraJumps;
    }

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        movement = playerInputActions.Player.Movement;
        movement.Enable();

        playerInputActions.Player.Jump.performed += Jump;
        playerInputActions.Player.Jump.Enable();
    }


    private void OnDisable()
    {
        movement.Disable();
        playerInputActions.Player.Jump.Disable();
    }

    private void FixedUpdate()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // Updates vertical velocity (up relative to player) to allow gravity and prevent phasing through objects
        if (verticalVelocity < 0 && isGrounded)
        {
            verticalVelocity = defaultVelocity;
            extraJumpsLeft = maxExtraJumps;
        }
        else if (verticalVelocity < terminalVelocity)
            verticalVelocity = terminalVelocity;
        verticalVelocity -= gravity * Time.deltaTime;

        // Calculate total displacement
        Vector3 displacement = Walk(movement.ReadValue<Vector2>());
        displacement += Time.deltaTime * verticalVelocity * transform.up;

        // Apply displacement
        rb.MovePosition(transform.position + displacement);
    }

    // Translates 2D input into 3D displacement
    public Vector3 Walk(Vector2 direction)
    {
        Vector3 movement = direction.x * transform.right + direction.y * transform.forward;
        return movement.normalized * Time.deltaTime * speed;
    }

    public void Jump(InputAction.CallbackContext obj)
    {
        if (isGrounded)
        {
            verticalVelocity = jumpSpeed;
        }
        else if (extraJumpsLeft > 0)
        {
            extraJumpsLeft--;
            verticalVelocity = jumpSpeed * 0.75f;
        }
    }
}
