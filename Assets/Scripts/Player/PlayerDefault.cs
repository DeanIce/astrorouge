using Gravity;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDefault : MonoBehaviour, IPlayer
{
    private const float groundDistance = 0.1f;

    // Dynamic player info
    [SerializeField] private int extraJumpsLeft;
    [SerializeField] private float jumpForce = 32f;
    [SerializeField] [Range(0.5f, 1.0f)] private float extraJumpDampaner = 0.8f;
    private readonly int maxExtraJumps = 2; // Total jumps = maxExtraJumps + 1
    private readonly float sprintSpeed = 10f;
    private readonly float turnSpeed = Mathf.PI / 3.0f;

    // Player stats
    private readonly float walkSpeed = 6f;
    private Transform groundCheck;
    private LayerMask groundMask;
    private bool isGrounded;
    private bool isSprinting;
    private InputAction movement, look;

    // Constants
    private PlayerInputActions playerInputActions;
    private Rigidbody rb;

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
        playerInputActions.Player.MeleeAttack.performed += MeleeAttack;
        playerInputActions.Player.MeleeAttack.Enable();
        playerInputActions.Player.RangedAttack.performed += RangedAttack;
        playerInputActions.Player.RangedAttack.Enable();
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
        return Vector3.RotateTowards(transform.forward, transform.right * Mathf.Sign(direction.x),
            turnSpeed * Time.deltaTime * Mathf.Abs(direction.x), 0.0f);
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
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
        else if (extraJumpsLeft > 0)
        {
            extraJumpsLeft--;
            rb.AddForce(transform.up * jumpForce * extraJumpDampaner, ForceMode.Impulse);
        }
    }

    public void SprintToggle(InputAction.CallbackContext obj)
    {
        isSprinting = !isSprinting;
    }

    public void MeleeAttack(InputAction.CallbackContext obj)
    {

    }

    public void RangedAttack(InputAction.CallbackContext obj)
    {

    }
}