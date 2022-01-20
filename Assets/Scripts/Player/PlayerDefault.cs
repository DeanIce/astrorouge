using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDefault : MonoBehaviour, IPlayer
{
    private PlayerInputActions playerInputActions;
    private InputAction movement, look;
    private Rigidbody rb;

    // Player stats
    private float turnSpeed = 4f;
    private float walkSpeed = 6f;
    private float sprintSpeed = 10f;
    private float jumpSpeed = 15f;
    private float gravity = 45f;
    private int maxExtraJumps = 2; // Total jumps = maxExtraJumps + 1

    // Dynamic player info
    public float verticalVelocity;
    private bool isGrounded;
    private int extraJumpsLeft;
    private bool isSprinting;

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
        look = playerInputActions.Player.Look;
        look.Enable();

        playerInputActions.Player.Jump.performed += Jump;
        playerInputActions.Player.Jump.Enable();
        playerInputActions.Player.Sprint.started += SprintToggle;
        playerInputActions.Player.Sprint.canceled += SprintToggle;
        playerInputActions.Player.Sprint.Enable();
    }


    private void OnDisable()
    {
        movement.Disable();
        look.Disable();
        playerInputActions.Player.Jump.Disable();
        playerInputActions.Player.Sprint.Disable();
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

        // Calculate lookAt vector then increment quaternion
        Vector3 lookAt = Look(look.ReadValue<Vector2>());
        Quaternion rotation = transform.rotation * Quaternion.FromToRotation(transform.forward, lookAt);

        // Apply rotation
        rb.MoveRotation(rotation);
    }

    // Translates 2D input into 3D looking direction
    public Vector3 Look(Vector2 direction)
    {
        Vector3 change = direction.x * transform.right;
        return turnSpeed * Time.deltaTime * change + transform.forward;
    }

    // Translates 2D input into 3D displacement
    public Vector3 Walk(Vector2 direction)
    {
        Vector3 movement = direction.x * transform.right + direction.y * transform.forward;
        return (isSprinting ? sprintSpeed : walkSpeed) * Time.deltaTime * movement.normalized;
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

    public void SprintToggle(InputAction.CallbackContext obj)
    {
        isSprinting = !isSprinting;
    }
}
