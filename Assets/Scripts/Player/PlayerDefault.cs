using Gravity;
using Managers;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDefault : MonoBehaviour, IPlayer
{
    // Constants
    private const float groundDistance = 0.1f;

    // Dynamic Player Info
    [SerializeField] private int extraJumpsLeft;

    // Inspector values
    [SerializeField] public float sensitivity = 0.2f;
    [SerializeField] private GameObject followTarget;
    [SerializeField] private GameObject fireLocation;

    private Animator animator;
    private Direction dir;
    private Transform groundCheck;
    private LayerMask groundMask;
    private bool isGrounded;
    private InputAction movement, look;
    private Direction oldDir;

    // References
    private Rigidbody rb;
    protected internal bool useGravity = true;

    public bool IsSprinting { get; private set; }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        groundCheck = transform.Find("GroundCheck");
        groundMask = LayerMask.GetMask("Ground");
        extraJumpsLeft = PlayerStats.Instance.maxExtraJumps;

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        // Gravity
        var sumForce = GravityManager.GetGravity(transform.position, out var upAxis);

        if (useGravity) rb.AddForce(sumForce * Time.deltaTime);


        // print(sumForce);
        Debug.DrawLine(transform.position, sumForce, Color.blue);

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded) extraJumpsLeft = PlayerStats.Instance.maxExtraJumps;

        // By far the easiest solution for monitoring 'grounded-ness' for animation tree.
        animator.SetBool("isGrounded", isGrounded);

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

        playerInputMap.PauseGame.performed += PauseGame;
        playerInputMap.PauseGame.Enable();

        playerInputMap.PrimaryAttack.performed += BasicAttack;
        playerInputMap.PrimaryAttack.Enable();
        playerInputMap.SecondaryAttack.performed += BeamAttack;
        playerInputMap.SecondaryAttack.Enable();
        playerInputMap.UtilityAction.performed += HitscanAttack;
        playerInputMap.UtilityAction.Enable();
        playerInputMap.SpecialAction.performed += LobAttack;
        playerInputMap.SpecialAction.Enable();
    }

    private void OnDisable()
    {
        var playerInputMap = InputManager.inputActions.Player;
        movement.Disable();
        look.Disable();

        playerInputMap.Jump.Disable();
        playerInputMap.Jump.performed -= Jump;

        playerInputMap.Sprint.Disable();
        playerInputMap.Sprint.started -= SprintToggle;
        playerInputMap.Sprint.canceled -= SprintToggle;

        playerInputMap.PauseGame.Disable();
        playerInputMap.PauseGame.performed -= PauseGame;

        playerInputMap.PrimaryAttack.Disable();
        playerInputMap.PrimaryAttack.performed -= BasicAttack;
        playerInputMap.SecondaryAttack.Disable();
        playerInputMap.SecondaryAttack.performed -= BeamAttack;
        playerInputMap.UtilityAction.Disable();
        playerInputMap.UtilityAction.performed -= HitscanAttack;
        playerInputMap.SpecialAction.Disable();
        playerInputMap.SpecialAction.performed -= LobAttack;
    }


    private void OnDrawGizmos()
    {
        if (groundCheck) Gizmos.DrawSphere(groundCheck.position, groundDistance);
    }

    // Translates 2D input into 3D looking direction
    public Vector3 Look(Vector2 direction)
    {
        return Vector3.RotateTowards(transform.forward, transform.right * Mathf.Sign(direction.x),
            sensitivity * Time.deltaTime * Mathf.Abs(direction.x), 0.0f);
    }

    // Translates 2D input into 3D displacement
    public Vector3 Walk(Vector2 direction)
    {
        HandleMoveAnimation(direction);

        var movement = direction.x * transform.right + direction.y * transform.forward;
        return (IsSprinting ? PlayerStats.Instance.sprintMultiplier : 1) * PlayerStats.Instance.movementSpeed *
               Time.deltaTime * movement.normalized;
    }

    public void Jump(InputAction.CallbackContext obj)
    {
        if (isGrounded)
        {
            rb.AddForce(PlayerStats.Instance.jumpForce * transform.up, ForceMode.Impulse);
            HandleJumpAnimation();
        }
        else if (extraJumpsLeft > 0)
        {
            extraJumpsLeft--;
            rb.velocity = Vector3.zero; // Resets force applied to rb (So double jumps feel good)
            rb.AddForce(PlayerStats.Instance.extraJumpDampaner * PlayerStats.Instance.jumpForce * transform.up,
                ForceMode.Impulse);
        }
    }

    public void SprintToggle(InputAction.CallbackContext obj)
    {
        IsSprinting = !IsSprinting;
    }

    public void TakeDmg(float dmg)
    {
        // Temp, add damage negation and other maths here later.
        PlayerStats.Instance.currentHealth -= dmg;
        EventManager.Instance.runStats.damageTaken += dmg;
        //Doesn't actually matter once we implement game over
        if (PlayerStats.Instance.currentHealth < 0) PlayerStats.Instance.currentHealth = 0;

        gameObject.GetComponent<HudUI>().SetHealth(PlayerStats.Instance.currentHealth);
        if (PlayerStats.Instance.currentHealth <= 0f) Die();
    }

    public void Die()
    {
        // Trigger death events.
        HandleDeathAnimation();

        // Todo: Initialize new script to handle death, remove this script from player.
        GetComponent<HandleDeath>().enabled = true;
        GetComponent<PlayerDefault>().enabled = false;
    }

    private void PauseGame(InputAction.CallbackContext obj)
    {
        EventManager.Instance.Pause();
    }

    private void BasicAttack(InputAction.CallbackContext obj)
    {
        _ = HandleEffects(ProjectileFactory.Instance.CreateBasicProjectile(fireLocation.transform.position,
            PlayerStats.Instance.rangeProjectileSpeed * AttackVector(),
            LayerMask.GetMask("Enemy", "Ground"),
            PlayerStats.Instance.rangeProjectileRange / PlayerStats.Instance.rangeProjectileSpeed,
            PlayerStats.Instance.GetRangeDamage()));
    }

    private void BeamAttack(InputAction.CallbackContext obj)
    {
        _ = HandleEffects(ProjectileFactory.Instance.CreateBeamProjectile(fireLocation.transform.position,
            AttackVector(),
            LayerMask.GetMask("Enemy", "Ground"),
            LayerMask.GetMask("Ground"),
            0.5f, // TODO (Simon): Mess with value
            PlayerStats.Instance.GetRangeDamage(),
            PlayerStats.Instance.rangeProjectileRange));
    }

    private void HitscanAttack(InputAction.CallbackContext obj)
    {
        _ = HandleEffects(ProjectileFactory.Instance.CreateHitscanProjectile(fireLocation.transform.position,
            AttackVector(),
            LayerMask.GetMask("Enemy", "Ground"),
            PlayerStats.Instance.GetRangeDamage(),
            PlayerStats.Instance.rangeProjectileRange));
    }

    private void LobAttack(InputAction.CallbackContext obj)
    {
        var attackVec = AttackVector();
        var liftVec = transform.up - Vector3.Project(transform.up, attackVec);

        var projectile = ProjectileFactory.Instance.CreateGravityProjectile(transform.position + transform.forward,
            PlayerStats.Instance.rangeProjectileSpeed * (attackVec + liftVec).normalized,
            LayerMask.GetMask("Enemy", "Ground"),
            PlayerStats.Instance.rangeProjectileRange / PlayerStats.Instance.rangeProjectileSpeed,
            PlayerStats.Instance.GetRangeDamage());
        HandleEffects(projectile);
    }

    private Vector3 AttackVector()
    {
        Vector2 screenCenterPoint = new(Screen.width / 2f, Screen.height / 2f + 32); // Magic number: 32
        var ray = Camera.main.ScreenPointToRay(screenCenterPoint);

        return (ray.GetPoint(PlayerStats.Instance.rangeProjectileRange) - fireLocation.transform.position).normalized;
    }

    private GameObject HandleEffects(GameObject projectile)
    {
        var rand = Random.Range(0.0f, 1.0f);

        if (rand < PlayerStats.Instance.burnChance) ProjectileFactory.Instance.AddBurn(projectile);
        rand = Random.Range(0.0f, 1.0f);
        if (rand < PlayerStats.Instance.poisonChance) ProjectileFactory.Instance.AddPoison(projectile);
        rand = Random.Range(0.0f, 1.0f);
        if (rand < PlayerStats.Instance.lightningChance) ProjectileFactory.Instance.AddLightning(projectile);
        rand = Random.Range(0.0f, 1.0f);
        if (rand < PlayerStats.Instance.radioactiveChance) ProjectileFactory.Instance.AddRadioactive(projectile);
        rand = Random.Range(0.0f, 1.0f);
        if (rand < PlayerStats.Instance.smiteChance) ProjectileFactory.Instance.AddSmite(projectile);
        rand = Random.Range(0.0f, 1.0f);
        if (rand < PlayerStats.Instance.slowChance) ProjectileFactory.Instance.AddSlow(projectile);
        rand = Random.Range(0.0f, 1.0f);
        if (rand < PlayerStats.Instance.stunChance) ProjectileFactory.Instance.AddStun(projectile);
        rand = Random.Range(0.0f, 1.0f);
        if (rand < PlayerStats.Instance.martyrdomChance) ProjectileFactory.Instance.AddMartyrdom(projectile);
        rand = Random.Range(0.0f, 1.0f);
        if (rand < PlayerStats.Instance.igniteChance) ProjectileFactory.Instance.AddIgnite(projectile);

        return projectile;
    }

    private void HandleMoveAnimation(Vector2 direction)
    {
        var threshold = 0.05f;

        // bit representation is concatenation of booleans Forward?, Back?, Right?, Left?
        dir = Direction.IDLE;
        if (direction.y > threshold)
            dir |= Direction.FORWARD;
        else if (direction.y < -threshold) dir |= Direction.BACKWARD;

        if (direction.x > threshold)
            dir |= Direction.RIGHT;
        else if (direction.x < -threshold) dir |= Direction.LEFT;

        if (dir != oldDir)
        {
            switch (dir)
            {
                case Direction.FORWARD:
                    animator.SetBool("isWalkingForward", true);
                    break;
                case Direction.FORWARDLEFT:
                    animator.SetBool("isWalkingForwardLeft", true);
                    break;
                case Direction.FORWARDRIGHT:
                    animator.SetBool("isWalkingForwardRight", true);
                    break;
                case Direction.LEFT:
                    animator.SetBool("isWalkingLeft", true);
                    break;
                case Direction.RIGHT:
                    animator.SetBool("isWalkingRight", true);
                    break;
                case Direction.BACKWARD:
                    animator.SetBool("isWalkingBackward", true);
                    break;
                case Direction.BACKLEFT:
                    animator.SetBool("isWalkingBackLeft", true);
                    break;
                case Direction.BACKRIGHT:
                    animator.SetBool("isWalkingBackRight", true);
                    break;
                case Direction.IDLE:
                    animator.SetBool("isIdle", true);
                    break;
            }

            switch (oldDir)
            {
                case Direction.FORWARD:
                    animator.SetBool("isWalkingForward", false);
                    break;
                case Direction.FORWARDLEFT:
                    animator.SetBool("isWalkingForwardLeft", false);
                    break;
                case Direction.FORWARDRIGHT:
                    animator.SetBool("isWalkingForwardRight", false);
                    break;
                case Direction.LEFT:
                    animator.SetBool("isWalkingLeft", false);
                    break;
                case Direction.RIGHT:
                    animator.SetBool("isWalkingRight", false);
                    break;
                case Direction.BACKWARD:
                    animator.SetBool("isWalkingBackward", false);
                    break;
                case Direction.BACKLEFT:
                    animator.SetBool("isWalkingBackLeft", false);
                    break;
                case Direction.BACKRIGHT:
                    animator.SetBool("isWalkingBackRight", false);
                    break;
                case Direction.IDLE:
                    animator.SetBool("isIdle", false);
                    break;
            }

            oldDir = dir;

            //See direction states of player: Debug.Log("Current State: " + dir);
        }

        animator.SetBool("isRunning", IsSprinting);
    }

    private void HandleJumpAnimation()
    {
        animator.SetTrigger("isJumping");
    }

    private void HandleDeathAnimation()
    {
        animator.SetBool("isAlive", false);
    }

    // Constants

    // Animation Enumerator
    // bit representation is Forward,Back,Right,Left
    private enum Direction
    {
        IDLE = 0b0000,
        FORWARD = 0b1000,
        BACKWARD = 0b0100,
        LEFT = 0b0001,
        RIGHT = 0b0010,
        FORWARDLEFT = 0b1001,
        FORWARDRIGHT = 0b1010,
        BACKLEFT = 0b0101,
        BACKRIGHT = 0b0110
    }
}