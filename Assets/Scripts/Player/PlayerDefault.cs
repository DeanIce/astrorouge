using Gravity;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDefault : MonoBehaviour, IPlayer
{
    private const float groundDistance = 0.1f;
    [SerializeField] private float jumpForce = 30f;
    private int extraJumpsLeft;

    // Constants
    private Transform groundCheck;
    private LayerMask groundMask;

    // Dynamic player info
    private float impulseJumpForce;
    private bool isGrounded;
    private bool isSprinting;
    private readonly int maxExtraJumps = 2; // Total jumps = maxExtraJumps + 1
    private InputAction movement, look;
    private PlayerInputActions playerInputActions;
    private Rigidbody rb;
    private readonly float sprintSpeed = 10f;

    // Player stats
    private readonly float turnSpeed = 4f;
    private readonly float walkSpeed = 6f;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        groundCheck = transform.Find("GroundCheck");
        groundMask = LayerMask.GetMask("Ground");
        extraJumpsLeft = maxExtraJumps;
    }

    private void FixedUpdate()
    {
        // Gravity
        var sumForce = GravityManager.GetGravity(transform.position, out var upAxis);
        rb.AddForce(sumForce * Time.deltaTime);
        Debug.DrawLine(transform.position, sumForce, Color.blue);

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded)
            extraJumpsLeft = maxExtraJumps;

        // Updates vertical velocity (up relative to player) to allow gravity and prevent phasing through objects
        if (impulseJumpForce > 0.1f)
        {
            rb.AddForce(transform.up * impulseJumpForce, ForceMode.Impulse);
            impulseJumpForce = 0;
        }

        // Calculate total displacement
        var displacement = Walk(movement.ReadValue<Vector2>());
        rb.MovePosition(transform.position + displacement);

        // Calculate lookAt vector then increment quaternion
        var lookAt = Look(look.ReadValue<Vector2>());

        // Apply rotation
        rb.MoveRotation(Quaternion.FromToRotation(transform.up, upAxis) *
                        Quaternion.FromToRotation(transform.forward, lookAt) * transform.rotation);
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

    // Translates 2D input into 3D looking direction
    public Vector3 Look(Vector2 direction)
    {
        var change = direction.x * transform.right;
        return turnSpeed * Time.deltaTime * change + transform.forward;
    }

    // Translates 2D input into 3D displacement
    public Vector3 Walk(Vector2 direction)
    {
        var movement = direction.x * transform.right + direction.y * transform.forward;
        return (isSprinting ? sprintSpeed : walkSpeed) * Time.deltaTime * movement.normalized;
    }

    public void Jump(InputAction.CallbackContext obj)
    {
        if (isGrounded)
        {
            impulseJumpForce = jumpForce;
        }
        else if (extraJumpsLeft > 0)
        {
            extraJumpsLeft--;
            impulseJumpForce = jumpForce * 0.75f;
        }
    }

    public void SprintToggle(InputAction.CallbackContext obj)
    {
        isSprinting = !isSprinting;
    }
}