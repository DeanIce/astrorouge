using Gravity;
using Managers;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDefault : MonoBehaviour, IPlayer
{
    // Constants
    private const float groundDistance = 0.1f;

    // Dynamic Player Info
    [SerializeField] private int extraJumpsLeft;
    [SerializeField] private float primaryAttackDelay;
    [SerializeField] private float secondaryAttackDelay;

    // Inspector values
    [SerializeField] public float sensitivity = 0.2f;
    [SerializeField] private GameObject followTarget;
    [SerializeField] private GameObject fireLocation;
    [SerializeField] private AudioClip attack1SoundEffect;
    [SerializeField] private AudioClip attack2SoundEffect;
    [SerializeField] private float spread = 1.0f;

    public HudUI hudUI;
    private readonly float decreasePerSecond = 60f;
    private readonly float increasePerSecond = 60f;
    private readonly float maxSpread = 30f;
    private readonly float minSpread = 1f;


    private Animator animator;

    private float crosshairSpread = 1f;
    private Transform groundCheck;
    private LayerMask groundMask;
    private bool isFiring;
    private bool isGrounded;
    private bool isPrimaryAttacking;
    private InputAction movement, look;

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

    public void Update()
    {
        // Adjust delay timers
        primaryAttackDelay = primaryAttackDelay < 0 ? primaryAttackDelay : primaryAttackDelay - Time.deltaTime;
        secondaryAttackDelay = secondaryAttackDelay < 0 ? secondaryAttackDelay : secondaryAttackDelay - Time.deltaTime;

        if (isPrimaryAttacking && primaryAttackDelay < 0)
        {
            BasicAttack();
            primaryAttackDelay = PlayerStats.Instance.rangeAttackDelay;
        }
    }

    private void FixedUpdate()
    {
        // Gravity
        Vector3 sumForce = GravityManager.GetGravity(transform.position, out Vector3 upAxis);

        if (useGravity) rb.AddForce(sumForce * Time.deltaTime);


        // print(sumForce);
        Debug.DrawLine(transform.position, sumForce, Color.blue);

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded) extraJumpsLeft = PlayerStats.Instance.maxExtraJumps;

        // By far the easiest solution for monitoring 'grounded-ness' for animation tree.
        animator.SetBool("isGrounded", isGrounded);

        // Calculate total displacement
        Vector3 displacement = Walk(movement.ReadValue<Vector2>());
        rb.MovePosition(transform.position + displacement);

        // Calculate lookAt vector then increment quaternion
        Vector3 lookAt = Look(look.ReadValue<Vector2>());

        // Apply rotation
        rb.MoveRotation(Quaternion.FromToRotation(transform.up, upAxis) *
                        Quaternion.FromToRotation(transform.forward, lookAt) * transform.rotation);
        if (IsSprinting)
            crosshairSpread += increasePerSecond * Time.deltaTime;
        else
            crosshairSpread -= decreasePerSecond * Time.deltaTime;
        crosshairSpread = Mathf.Clamp(crosshairSpread, minSpread, maxSpread);
        if (hudUI) hudUI.AdjustCrosshair(crosshairSpread);
    }

    private void OnEnable()
    {
        PlayerInputActions.PlayerActions playerInputMap = InputManager.inputActions.Player;

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

        playerInputMap.PrimaryAttack.started += PrimaryAttackToggle;
        playerInputMap.PrimaryAttack.canceled += PrimaryAttackToggle;
        playerInputMap.PrimaryAttack.Enable();
        playerInputMap.SecondaryAttack.performed += SecondaryAttack;
        playerInputMap.SecondaryAttack.Enable();
        playerInputMap.UtilityAction.performed += HitscanAttack;
        playerInputMap.UtilityAction.Enable();
        playerInputMap.SpecialAction.performed += LobAttack;
        playerInputMap.SpecialAction.Enable();
    }

    private void OnDisable()
    {
        PlayerInputActions.PlayerActions playerInputMap = InputManager.inputActions.Player;
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
        playerInputMap.PrimaryAttack.started -= PrimaryAttackToggle;
        playerInputMap.PrimaryAttack.canceled -= PrimaryAttackToggle;
        playerInputMap.SecondaryAttack.Disable();
        playerInputMap.SecondaryAttack.performed -= SecondaryAttack;
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

        Vector3 movement = direction.x * transform.right + direction.y * transform.forward;
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
        animator.SetTrigger("takeDamage");
        // Temp, add damage negation and other maths here later.
        PlayerStats.Instance.currentHealth -= dmg;
        if (PlayerStats.Instance.currentHealth > 0) EventManager.Instance.runStats.damageTaken += dmg;
        //Doesn't actually matter once we implement game over
        if (PlayerStats.Instance.currentHealth < 0) PlayerStats.Instance.currentHealth = 0;

        if (hudUI) hudUI.SetHealth(PlayerStats.Instance.currentHealth);
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

    private void PrimaryAttackToggle(InputAction.CallbackContext obj)
    {
        isPrimaryAttacking = !isPrimaryAttacking;
    }

    private void SecondaryAttack(InputAction.CallbackContext obj)
    {
        if (secondaryAttackDelay > 0) return;
        secondaryAttackDelay = PlayerStats.Instance.meleeAttackDelay;

        BeamAttack();
    }

    private void BasicAttack()
    {
        _ = HandleEffects(ProjectileFactory.Instance.CreateBasicProjectile(fireLocation.transform.position,
            PlayerStats.Instance.rangeProjectileSpeed * AttackVector(),
            LayerMask.GetMask("Enemy", "Ground"),
            PlayerStats.Instance.rangeProjectileRange / PlayerStats.Instance.rangeProjectileSpeed,
            PlayerStats.Instance.GetRangeDamage()));
        AudioManager.Instance.PlaySFX(attack1SoundEffect, 0.1f);
    }

    private void BeamAttack()
    {
        _ = HandleEffects(ProjectileFactory.Instance.CreateBeamProjectile(fireLocation.transform.position,
            AttackVector(),
            LayerMask.GetMask("Enemy", "Ground"),
            LayerMask.GetMask("Ground"),
            0.2f, // TODO (Simon): Mess with value
            PlayerStats.Instance.GetRangeDamage(),
            PlayerStats.Instance.rangeProjectileRange));
        AudioManager.Instance.PlaySFX(attack2SoundEffect, 0.3f);
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
        animator.SetTrigger("lobThrow");
        Vector3 attackVec = AttackVector();
        Vector3 liftVec = transform.up - Vector3.Project(transform.up, attackVec);

        _ = HandleEffects(ProjectileFactory.Instance.CreateGravityProjectile(transform.position + transform.forward,
            10 * (attackVec + liftVec).normalized, //TODO (Simon): Fix magic number 10
            LayerMask.GetMask("Enemy", "Ground"),
            PlayerStats.Instance.rangeProjectileRange / 10, //TODO (Simon): Fix magic number 10
            PlayerStats.Instance.GetRangeDamage()));
    }

    private Vector3 AttackVector()
    {
        float bulletSpread = spread;
        if (IsSprinting)
            bulletSpread += 1.5f;

        Vector2 screenCenterPoint = new(Screen.width / 2f, Screen.height / 2f + 32); // Magic number: 32
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);

        var screenAim = new Vector3(screenCenterPoint.x, screenCenterPoint.y, 30f);

        Vector3 centerPos = Camera.main.ScreenToWorldPoint(screenAim);
        Vector3 spreadPos = Random.insideUnitCircle * bulletSpread;
        Vector3 screenPos = Camera.main.WorldToScreenPoint(centerPos + spreadPos);
        Ray ray2 = Camera.main.ScreenPointToRay(screenPos);
        Debug.DrawRay(ray2.origin, ray2.direction * 50f, Color.green, 1f);
        Debug.DrawRay(ray.origin, ray.direction * 50f, Color.red, 1f);

        return (ray2.GetPoint(PlayerStats.Instance.rangeProjectileRange) - fireLocation.transform.position).normalized;
    }

    private Vector3 AttackVectorWithRecoil()
    {
        Vector2 screenCenterPoint = new(Screen.width / 2f, Screen.height / 2f + 32); // Magic number: 32
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);

        return (ray.GetPoint(PlayerStats.Instance.rangeProjectileRange) - fireLocation.transform.position).normalized;
    }

    private GameObject HandleEffects(GameObject projectile)
    {
        float rand = Random.Range(0.0f, 1.0f);

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
        Vector2 moveDir = direction.normalized;

        animator.SetFloat("horizontalMovement", moveDir.x, 0.1f, Time.deltaTime);
        animator.SetFloat("verticalMovement", moveDir.y, 0.1f, Time.deltaTime);
        animator.SetBool("isSprinting", IsSprinting);
    }

    private void HandleJumpAnimation()
    {
        animator.SetTrigger("isJumping");
    }

    private void HandleDeathAnimation()
    {
        animator.SetBool("isAlive", false);
    }
}