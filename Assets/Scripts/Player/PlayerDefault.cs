using System.Collections;
using Gravity;
using Managers;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDefault : MonoBehaviour, IPlayer
{
    // for testing attack purposes
    public MeshRenderer meleeMeshRenderer;

    // Dynamic Player Info
    [SerializeField] private int extraJumpsLeft;
    private bool isGrounded;
    private bool isSprinting;

    // References
    private Rigidbody rb;
    private Transform groundCheck;
    private LayerMask groundMask;
    private LayerMask enemyMask;
    private InputAction movement, look;

    // Constants
    private const float groundDistance = 0.1f;
    private const float turnSpeed = Mathf.PI / 3.0f;
    [SerializeField] private float sensitivity = 0.2f;

    public GameObject followTarget;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        groundCheck = transform.Find("GroundCheck");
        groundMask = LayerMask.GetMask("Ground");
        enemyMask = LayerMask.GetMask("Enemy");
        extraJumpsLeft = PlayerStats.Instance.maxExtraJumps;

        //Temporary placement
        
    }

    private void FixedUpdate()
    {
        // Gravity
        var sumForce = GravityManager.GetGravity(transform.position, out var upAxis);
        rb.AddForce(sumForce * Time.deltaTime);
        // print(sumForce);
        Debug.DrawLine(transform.position, sumForce, Color.blue);

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded)
            extraJumpsLeft = PlayerStats.Instance.maxExtraJumps;

        // Calculate total displacement
        var displacement = Walk(movement.ReadValue<Vector2>());
        rb.MovePosition(transform.position + displacement);

       
        //Rotates Follow Target
        followTarget.transform.rotation *= Quaternion.AngleAxis(look.ReadValue<Vector2>().x * sensitivity, Vector3.up);

        followTarget.transform.rotation *= Quaternion.AngleAxis(look.ReadValue<Vector2>().y * sensitivity, Vector3.right);

        var eAngles = followTarget.transform.localEulerAngles;
        eAngles.z = 0;

        var eAngleX = followTarget.transform.localEulerAngles.x;

        if (eAngleX > 180 && eAngleX < 340)
        {
            eAngles.x = 340;
        }
        else if (eAngleX < 180 && eAngleX > 40)
        {
            eAngles.x = 40;
        }

        followTarget.transform.localEulerAngles = eAngles;    
 
        // Calculate lookAt vector then increment quaternion
        var lookAt = Look(look.ReadValue<Vector2>());

        // Apply rotation
        rb.MoveRotation(Quaternion.FromToRotation(transform.up, upAxis) *
                       Quaternion.FromToRotation(transform.forward, lookAt) * transform.rotation);

        followTarget.transform.localEulerAngles = new Vector3(eAngles.x, 0, 0);

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

        playerInputMap.MeleeAttack.performed += MeleeAttack;
        playerInputMap.MeleeAttack.Enable();

        playerInputMap.RangedAttack.performed += RangedAttack;
        playerInputMap.RangedAttack.Enable();
    }

    private void OnDisable()
    {
        var playerInputMap = InputManager.inputActions.Player;
        movement.Disable();
        look.Disable();
        playerInputMap.Sprint.Disable();
        playerInputMap.Jump.performed -= Jump;

        playerInputMap.PauseGame.Disable();
        playerInputMap.PauseGame.performed -= PauseGame;

        playerInputMap.MeleeAttack.Disable();
        playerInputMap.MeleeAttack.performed -= MeleeAttack;
        playerInputMap.RangedAttack.Disable();
        playerInputMap.RangedAttack.performed -= RangedAttack;
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
        var movement = direction.x * transform.right + direction.y * transform.forward;
        return (isSprinting ? PlayerStats.Instance.sprintMultiplier : 1) * PlayerStats.Instance.movementSpeed *
               Time.deltaTime * movement.normalized;
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
            rb.AddForce(PlayerStats.Instance.extraJumpDampaner * PlayerStats.Instance.jumpForce * transform.up,
                ForceMode.Impulse);
        }
    }

    public void SprintToggle(InputAction.CallbackContext obj)
    {
        isSprinting = !isSprinting;
    }

    private void PauseGame(InputAction.CallbackContext obj)
    {
        EventManager.instance.Pause();
    }

    public void MeleeAttack(InputAction.CallbackContext obj)
    {
        // TODO (Simon): Move beam attack into different action
        ProjectileFactory.Instance.CreateBeamProjectile(transform.position + transform.forward,
            transform.forward,
            LayerMask.GetMask("Enemy", "Ground"),
            LayerMask.GetMask("Ground"),
            0.1f, // TODO (Simon): Mess with value
            PlayerStats.Instance.GetRangeDamage(),
            PlayerStats.Instance.rangeProjectileRange);

        // TODO (Simon): Figure out alternate melee attack
        /*hits = Physics.RaycastAll(transform.position, transform.forward, PlayerStats.Instance.meleeAttackRange,
            enemyMask);
        StartCoroutine(Attack());
        
         if (hits.Length != 0)
            //check for an enemy in the things the ray hit by whether it has an IEnemy
            foreach (var hit in hits)
                if (hit.collider.gameObject.GetComponent<IEnemy>() != null)
                    hit.collider.gameObject.GetComponent<IEnemy>().TakeDmg(5);
         */
    }

    public void RangedAttack(InputAction.CallbackContext obj)
    {
        // TESTING BURN PROJECTILES originally --> CreateBasicProjectile
        ProjectileFactory.Instance.CreateBurnProjectile(transform.position + transform.forward,
            PlayerStats.Instance.rangeProjectileSpeed * transform.forward,
            LayerMask.GetMask("Enemy", "Ground"),
            PlayerStats.Instance.rangeProjectileRange / PlayerStats.Instance.rangeProjectileSpeed,
            PlayerStats.Instance.GetRangeDamage());
    }

    private IEnumerator Attack()
    {
        meleeMeshRenderer.enabled = true;
        yield return new WaitForSeconds(0.5f);
        meleeMeshRenderer.enabled = false;
    }

    public void TakeDmg(float dmg)
    {
        // Temp, add damage negation and other maths here later.
        PlayerStats.Instance.currentHealth -= dmg;
        //Doesn't actually matter once we implement game over
        if (PlayerStats.Instance.currentHealth < 0)
            PlayerStats.Instance.currentHealth = 0;

        gameObject.GetComponent<HudUI>().SetHealth(PlayerStats.Instance.currentHealth);
        if (PlayerStats.Instance.currentHealth <= 0f)
        {
            //run end
        }
    }
}