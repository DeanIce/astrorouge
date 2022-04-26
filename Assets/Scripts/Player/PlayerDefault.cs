using System.Collections;
using Gravity;
using Managers;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDefault : MonoBehaviour, IPlayer
{
    // Constants
    private const float groundDistance = 0.1f;
    private const float inaccuracyPerSecond = 10f;
    private const float maxInaccuracy = 1.5f;
    private const float maxSpread = 40f;

    // Inspector set-able references
    [SerializeField] private GameObject followTarget;
    [SerializeField] private GameObject fireLocation;
    [SerializeField] private AudioClip attack1SoundEffect;
    [SerializeField] private AudioClip attack2SoundEffect;

    // Inspector set-able values
    [SerializeField] public float sensitivity = 0.2f;
    [SerializeField] private float baseSpread = 0.1f;
    [SerializeField] private float currentSpread = 1.0f;
    [SerializeField] private float sprintSpread = 0f;
    // Attack values
    [SerializeField] private float primaryAttackProcChance = 1f;

    [SerializeField] private float secondaryAttackDuration = 0.2f;
    [SerializeField] private float secondaryAttackTickTime = 0.02f;
    [SerializeField] private float secondaryAttackDamageMult = 4f;
    [SerializeField] private float secondaryAttackProcChance = 2f;
    [SerializeField] private float secondaryAttackCooldown = 5f;

    [SerializeField] private float specialActionDelay = 0.8f;
    [SerializeField] private float specialActionDamageMult = 24f;
    [SerializeField] private float specialActionProcChance = 0.7f;
    [SerializeField] private float specialActionCooldown = 8f;

    [SerializeField] private float utilityActionDelay = 0.2f;
    [SerializeField] private float utilityActionCooldown = 4f;
    [SerializeField] private float utilityActionDuration = 0.5f;

    [SerializeField] private float meleeAttackProcChance = 2f;

    // Misc values
    private Animator animator;

    // Dynamic Player Info
    private float crosshairSpread = 1f;
    private int extraJumpsLeft;
    private float globalAttackDealy;
    private Transform groundCheck;
    private LayerMask groundMask;
    private bool isGrounded;
    private bool isPrimaryAttacking;
    private bool jump;
    private Vector3 jumpForceVal;
    private InputAction movement, look;
    private Rigidbody rb;
    private float timeOfLastDamage;
    protected internal bool useGravity = true;
    private bool IframeActive = false;

    // Public Getters
    public bool IsSprinting { get; private set; }
    public bool IsDashing { get; private set; }
    public float PrimaryAttackDelay { get; private set; }
    public float SecondaryAttackDelay { get; private set; }
    public float MeleeAttackDelay { get; private set; }
    public float SpecialActionDelay { get; private set; }
    public float UtilityActionDelay { get; private set; }
    public float CurrentSpread { get; private set; }

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
        static float Decrement(float value)
        {
            return value < 0 ? value : value - Time.deltaTime;
        }

        PrimaryAttackDelay = Decrement(PrimaryAttackDelay);
        SecondaryAttackDelay = Decrement(SecondaryAttackDelay);
        MeleeAttackDelay = Decrement(MeleeAttackDelay);
        SpecialActionDelay = Decrement(SpecialActionDelay);
        UtilityActionDelay = Decrement(UtilityActionDelay);
        globalAttackDealy = Decrement(globalAttackDealy);
        CurrentSpread = Decrement(CurrentSpread);

        if (globalAttackDealy <= 0)
        {
            if (isPrimaryAttacking && PrimaryAttackDelay < 0)
            {
                CurrentSpread = CurrentSpread + (inaccuracyPerSecond * Time.deltaTime);
                CurrentSpread = Mathf.Clamp(CurrentSpread, baseSpread, maxInaccuracy);
                ProjectileAttack();
                PrimaryAttackDelay = PlayerStats.Instance.rangeAttackDelay;
            }
        }

        // Health regen
        if (Time.time - timeOfLastDamage > PlayerStats.Instance.regenDelay &&
            PlayerStats.Instance.IsAlive() &&
            PlayerStats.Instance.currentHealth < PlayerStats.Instance.maxHealth)
        {
            PlayerStats.Instance.currentHealth += PlayerStats.Instance.healthRegen * Time.deltaTime;
            Mathf.Clamp(PlayerStats.Instance.currentHealth, 0, PlayerStats.Instance.maxHealth);
            EventManager.Instance.PlayerStatsUpdated();
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

        // Apply jump force
        if (jump)
        {
            jump = false;
            rb.AddForce(jumpForceVal, ForceMode.Impulse);
        }

        // Calculate total displacement
        Vector3 displacement = Walk(movement.ReadValue<Vector2>());
        rb.MovePosition(transform.position + displacement);

        // Calculate lookAt vector then increment quaternion
        Vector3 lookAt = Look(look.ReadValue<Vector2>());

        // Apply rotation
        rb.MoveRotation(Quaternion.FromToRotation(transform.up, upAxis) *
                        Quaternion.FromToRotation(transform.forward, lookAt) * transform.rotation);

        crosshairSpread = maxSpread * (CurrentSpread / maxInaccuracy);

        if (IsSprinting)
            sprintSpread += 60f * Time.deltaTime;
        else
            sprintSpread -= 60f * Time.deltaTime;

        sprintSpread = Mathf.Clamp(sprintSpread, 0f, 20f);

        EventManager.Instance.CrosshairSpread(crosshairSpread+sprintSpread);
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
        playerInputMap.MeleeAttack.performed += MeleeAttack;
        playerInputMap.MeleeAttack.Enable();
        playerInputMap.UtilityAction.performed += UtilityAction; 
        playerInputMap.UtilityAction.Enable();
        playerInputMap.SpecialAction.performed += SpecialAction;
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
        playerInputMap.MeleeAttack.Disable();
        playerInputMap.MeleeAttack.performed -= MeleeAttack;
        playerInputMap.UtilityAction.Disable();
        playerInputMap.UtilityAction.performed -= UtilityAction;
        playerInputMap.SpecialAction.Disable();
        playerInputMap.SpecialAction.performed -= SpecialAction;
    }


    private void OnDrawGizmos()
    {
        if (groundCheck) Gizmos.DrawSphere(groundCheck.position, groundDistance);
    }

    // Translates 2D input into 3D looking direction
    public Vector3 Look(Vector2 direction)
    {
        float f = EventManager.Instance.user.lookSensitivity;
        direction *= new Vector2(f, f);
        return Vector3.RotateTowards(transform.forward, transform.right * Mathf.Sign(direction.x),
            sensitivity * Time.deltaTime * Mathf.Abs(direction.x), 0.0f);
    }

    // Translates 2D input into 3D displacement
    public Vector3 Walk(Vector2 direction)
    {
        HandleMoveAnimation(direction);

        Vector3 movement = direction.x * transform.right + direction.y * transform.forward;
        return (IsSprinting ? PlayerStats.Instance.sprintMultiplier : 1)* (IsDashing ? 5 : 1) * PlayerStats.Instance.movementSpeed *
               Time.deltaTime * movement.normalized;
    }

    public void Jump(InputAction.CallbackContext obj)
    {
        if (isGrounded)
        {
            jump = true;
            jumpForceVal = PlayerStats.Instance.jumpForce * transform.up;
            HandleJumpAnimation();
        }
        else if (extraJumpsLeft > 0)
        {
            extraJumpsLeft--;
            rb.velocity = Vector3.zero; // Resets force applied to rb (So double jumps feel good)
            jump = true;
            jumpForceVal = PlayerStats.Instance.extraJumpDampaner * PlayerStats.Instance.jumpForce * transform.up;
        }
    }

    public void SprintToggle(InputAction.CallbackContext obj)
    {
        IsSprinting = !IsSprinting;
    }

    public void TakeDmg(float dmg, int type = 0, bool isCrit = false)
    {
        AudioManager.Instance.PlayPlayerTakeDamage();
        timeOfLastDamage = Time.time;
        if (!IframeActive)
        {
            //Debug.Log($"");

            animator.SetTrigger("takeDamage");
            // Temp, add damage negation and other maths here later.
            float dmgAfterArmor = 0.0f;
            if (Random.value >= PlayerStats.Instance.dodgeChance)
            {
                dmgAfterArmor = dmg - dmg * ((float)PlayerStats.Instance.armor / 100f);
                if (dmgAfterArmor <= 0.0f) dmgAfterArmor = 1f;
            }
            else
            {
                AudioManager.Instance.PlayDodgeSqueak();
                Debug.Log("Dodged Damage");
            }

            PlayerStats.Instance.currentHealth -= dmgAfterArmor;
            EventManager.Instance.runStats.damageTaken += dmgAfterArmor;

            if (PlayerStats.Instance.currentHealth < 0)
            {
                EventManager.Instance.runStats.damageTaken += PlayerStats.Instance.currentHealth;
                PlayerStats.Instance.currentHealth = 0f;
            }

            StartCoroutine(beginIFrames());
        } //else { Debug.Log("got hit while invincible"); }

        EventManager.Instance.PlayerStatsUpdated();
        EventManager.Instance.PlayerDamaged((PlayerStats.Instance.maxHealth - PlayerStats.Instance.currentHealth) /
                                            PlayerStats.Instance.maxHealth);
        if (PlayerStats.Instance.currentHealth <= 0f) Die();
    }

    private IEnumerator beginIFrames()
    {
        IframeActive = true;
        yield return new WaitForSeconds(PlayerStats.Instance.invincibilityDuration * 0.001f); // converted to ms
        IframeActive = false;
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
        if (SecondaryAttackDelay > 0 || globalAttackDealy > 0) return;
        SecondaryAttackDelay = secondaryAttackCooldown;

        EventManager.Instance.SecondaryUsed(secondaryAttackCooldown);
        BeamAttack();
    }

    private void MeleeAttack(InputAction.CallbackContext obj)
    {
        if (MeleeAttackDelay > 0 || globalAttackDealy > 0) return;
        MeleeAttackDelay = PlayerStats.Instance.meleeAttackDelay;

        EventManager.Instance.MeleeUsed(MeleeAttackDelay);
        animator.SetTrigger("meleeAttack");
        _ = StartCoroutine(InstantaneousAttack(0.5f));
    }

    private void SpecialAction(InputAction.CallbackContext obj)
    {
        if (SpecialActionDelay > 0 || globalAttackDealy > 0) return;
        SpecialActionDelay = specialActionCooldown;

        EventManager.Instance.SpecialUsed(specialActionCooldown);
        _ = StartCoroutine(LobAttack());
    }

    private void UtilityAction(InputAction.CallbackContext obj)
    {
        if (UtilityActionDelay > 0 || globalAttackDealy > 0) return;
        UtilityActionDelay = utilityActionCooldown;

        EventManager.Instance.UtilityUsed(utilityActionCooldown);
        AudioManager.Instance.PlayDashWhoosh();
        _ = StartCoroutine(Dash());
    }

    private IEnumerator Dash()
    {
        IsDashing = true;
        globalAttackDealy = utilityActionDuration + 0.2f;
        yield return new WaitForSeconds(utilityActionDelay);
        IsDashing = false;
    }

    private void ProjectileAttack()
    {
        GameObject projectile = ProjectileFactory.Instance.CreateBasicProjectile(
            fireLocation.transform.position,
            PlayerStats.Instance.rangeProjectileSpeed * AttackVector(CurrentSpread),
            LayerMask.GetMask("Enemy", "Ground"),
            PlayerStats.Instance.rangeProjectileRange / PlayerStats.Instance.rangeProjectileSpeed,
            PlayerStats.Instance.GetRangeDamage());
        HandleEffects(projectile, primaryAttackProcChance);
        AudioManager.Instance.PlayShootBlaster();
    }

    private void BeamAttack()
    {
        globalAttackDealy = secondaryAttackDuration + 0.2f;

        float tickCount = secondaryAttackDuration / secondaryAttackTickTime;
        float beamDamage = PlayerStats.Instance.GetRangeDPS() * secondaryAttackDamageMult / tickCount;

        GameObject projectile = ProjectileFactory.Instance.CreateBeamProjectile(
            fireLocation.transform.position,
            AttackVector(),
            LayerMask.GetMask("Enemy", "Ground"),
            LayerMask.GetMask("Ground"),
            secondaryAttackDuration,
            secondaryAttackTickTime,
            beamDamage,
            PlayerStats.Instance.rangeProjectileRange);
        HandleEffects(projectile, secondaryAttackProcChance / tickCount);
        AudioManager.Instance.PlayShootBeam();
    }

    /*private void HitscanAttack(InputAction.CallbackContext obj)
    {
        _ = HandleEffects(
            ProjectileFactory.Instance.CreateHitscanProjectile(fireLocation.transform.position,
                AttackVector(),
                LayerMask.GetMask("Enemy", "Ground"),
                PlayerStats.Instance.GetRangeDamage(),
                PlayerStats.Instance.rangeProjectileRange),
            1f);
    }*/

    private IEnumerator LobAttack()
    {
        globalAttackDealy = 1.2f;

        animator.SetTrigger("lobThrow");
        yield return new WaitForSeconds(specialActionDelay);
        Vector3 attackVec = AttackVector();
        Vector3 liftVec = transform.up - Vector3.Project(transform.up, attackVec);

        Vector3 handOffset = transform.right * .3f + transform.up * .15f;

        GameObject projectile = ProjectileFactory.Instance.CreateGravityProjectile(
            transform.position + transform.forward + handOffset,
            transform.rotation,
            10f * (attackVec + liftVec).normalized, //TODO (Simon): Fix magic number 10
            LayerMask.GetMask("Enemy", "Ground"),
            PlayerStats.Instance.rangeProjectileRange / 10f, //TODO (Simon): Fix magic number 10
            PlayerStats.Instance.GetRangeDamage());
        HandleEffects(projectile, specialActionProcChance);
        _ = ProjectileFactory.Instance.AddExplosionOnDestroy(
            projectile,
            LayerMask.GetMask("Enemy", "Ground"),
            PlayerStats.Instance.GetRangeDamage() * specialActionDamageMult,
            3f * PlayerStats.Instance.rangeGrenadeSizeMultiplier);
    }

    private IEnumerator InstantaneousAttack(float attackDelay)
    {
        globalAttackDealy = 0.5f;

        yield return new WaitForSeconds(attackDelay);

        GameObject projectile = ProjectileFactory.Instance.CreateInstantaneousProjectile(
            transform.position + transform.forward * 0.5f,
            transform.rotation,
            PlayerStats.Instance.meleeAttackRange,
            LayerMask.GetMask("Enemy"),
            PlayerStats.Instance.GetMeleeDamage());
        HandleEffects(projectile, meleeAttackProcChance);
    }

    private Vector3 AttackVector(float bulletSpread = 0)
    {
        bulletSpread = IsSprinting ? bulletSpread + 1.0f : bulletSpread;

        Vector2 screenCenterPoint = new(Screen.width / 2f, Screen.height / 2f);
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

    /// <summary>
    ///     Applys status effects to provided projectile.
    /// </summary>
    /// <param name="projectile">The designated projectile.</param>
    /// <param name="procChance">The probability boost to apply an effect. (actual chance) = (proc chance) * (effect chance)</param>
    private void HandleEffects(GameObject projectile, float procChance)
    {
        float GetRandom()
        {
            return procChance * Random.Range(0.0f, 1.0f);
        }

        if (GetRandom() < PlayerStats.Instance.burnChance)
            ProjectileFactory.Instance.AddBurn(projectile);
        if (GetRandom() < PlayerStats.Instance.poisonChance)
            ProjectileFactory.Instance.AddPoison(projectile);
        if (GetRandom() < PlayerStats.Instance.lightningChance)
            ProjectileFactory.Instance.AddLightning(projectile);
        if (GetRandom() < PlayerStats.Instance.radioactiveChance)
            ProjectileFactory.Instance.AddRadioactive(projectile);
        if (GetRandom() < PlayerStats.Instance.smiteChance)
            ProjectileFactory.Instance.AddSmite(projectile);
        if (GetRandom() < PlayerStats.Instance.slowChance)
            ProjectileFactory.Instance.AddSlow(projectile);
        if (GetRandom() < PlayerStats.Instance.stunChance)
            ProjectileFactory.Instance.AddStun(projectile);
        if (GetRandom() < PlayerStats.Instance.martyrdomChance)
            ProjectileFactory.Instance.AddMartyrdom(projectile);
        if (GetRandom() < PlayerStats.Instance.igniteChance)
            ProjectileFactory.Instance.AddIgnite(projectile);
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
