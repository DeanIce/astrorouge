using Gravity;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDefault : MonoBehaviour, IPlayer
{
    private const float groundDistance = 0.1f;
    private const float turnSpeed = Mathf.PI / 3.0f;

    // Dynamic player info
    [SerializeField] private int extraJumpsLeft;
    private bool isGrounded;
    private bool isSprinting;

    // Constants
    private Rigidbody rb;
    private Transform groundCheck;
    private LayerMask groundMask;
    private InputAction movement, look;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        groundCheck = transform.Find("GroundCheck");
        groundMask = LayerMask.GetMask("Ground");
        extraJumpsLeft = PlayerStats.Instance.maxExtraJumps;
    }

    private void FixedUpdate()
    {
        // Gravity
        var sumForce = GravityManager.GetGravity(transform.position, out var upAxis);
        rb.AddForce(sumForce * Time.deltaTime);
        Debug.DrawLine(transform.position, sumForce, Color.blue);

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded)
            extraJumpsLeft = PlayerStats.Instance.maxExtraJumps;

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
        var playerInputMap = InputManager.inputActions.Player;

        movement = playerInputMap.Movement;
        movement.Enable();
        look = playerInputMap.Look;
        look.Enable();

        playerInputMap.Jump.performed += Jump;
        playerInputMap.Jump.Enable();
        playerInputMap.Sprint.started += SprintToggle;
        playerInputMap.Sprint.canceled += SprintToggle;
        playerInputMap.Sprint.Enable();
        playerInputMap.PauseGame.Enable();
    }

    private void OnDisable()
    {
        var playerInputMap = InputManager.inputActions.Player;
        movement.Disable();
        look.Disable();
        playerInputMap.Sprint.Disable();
        playerInputMap.Jump.performed -= Jump;
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
        return (isSprinting ? PlayerStats.Instance.sprintMultiplier : 1) * PlayerStats.Instance.movementSpeed * Time.deltaTime * movement.normalized;
    }

    public void Jump(InputAction.CallbackContext obj)
    {
        if (isGrounded)
        {
            rb.AddForce(PlayerStats.Instance.jumpForce * transform.up, ForceMode.Impulse);
        }
        else if (extraJumpsLeft > 0)
        {
            extraJumpsLeft--;
            rb.AddForce(PlayerStats.Instance.extraJumpDampaner * PlayerStats.Instance.jumpForce * transform.up, ForceMode.Impulse);
        }
    }

    public void SprintToggle(InputAction.CallbackContext obj)
    {
        isSprinting = !isSprinting;
    }
}