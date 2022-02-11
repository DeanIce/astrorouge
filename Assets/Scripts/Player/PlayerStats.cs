using UnityEngine;


public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; private set; }

    // Melee Stats
    public float meleeAttackDelay;
    public int meleeBaseDamage;
    public float meleeDamageMultiplier;
    public float meleeCritChance;
    public float meleeCritMultiplier;
    public float meleeKnockbackForce;
    public float meleeAttackRange;

    // Range Stats
    public float rangeAttackDelay;
    public int rangeBaseDamage;
    public float rangeDamageMultiplier;
    [Range(0.0f, 1.0f)] public float rangeCritChance;
    public float rangeCritMultiplier;
    public float rangeKnockbackForce; // TODO (Simon): Incorporate into 'collision' of Projectiles
    public float rangeProjectileRange;
    public float rangeProjectileSpeed;

    // Defense Stats
    public int maxHealth;
    public float currentHealth;
    public float healthRegen;
    public int armor;
    public float dodgeChance;
    public float invincibilityDuration; // How long player is immune to damage after getting hit

    // Movement Stats
    public int maxExtraJumps;
    public float jumpForce;
    public float extraJumpDampaner;
    public float movementSpeed;
    public float sprintMultiplier;
    public int dashCharges;
    public float dashDistance;
    public float dashRechargeRate;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            SetDefaultValues();
        }
    }

    public float GetRangeDamage()
    {
        if (Random.value <= rangeCritChance)
            return rangeBaseDamage * rangeDamageMultiplier * rangeCritMultiplier;
        else
            return rangeBaseDamage * rangeDamageMultiplier;
    }

    private void SetDefaultValues()
    {
        meleeAttackDelay = 3f;
        meleeBaseDamage = 1;
        meleeDamageMultiplier = 1f;
        meleeCritChance = 0.1f;
        meleeCritMultiplier = 2f;
        meleeKnockbackForce = 5f;
        meleeAttackRange = 1f;

        // Range Stats
        rangeAttackDelay = 2f;
        rangeBaseDamage = 1;
        rangeDamageMultiplier = 1f;
        rangeCritChance = 0.1f;
        rangeCritMultiplier = 2f;
        rangeKnockbackForce = 2f;
        rangeProjectileRange = 20f;
        rangeProjectileSpeed = 10f;

        // Defense Stats
        maxHealth = 100;
        currentHealth = maxHealth;
        healthRegen = 0.1f;
        armor = 1;
        dodgeChance = 0f;
        invincibilityDuration = 1f; // How long (seconds) player is immune to damage after getting hit

        // Movement Stats
        maxExtraJumps = 2;
        jumpForce = 32f;
        extraJumpDampaner = 0.8f;
        movementSpeed = 6f;
        sprintMultiplier = 1.5f;
        dashCharges = 0;
        dashDistance = 3f;
        dashRechargeRate = 1f / 10;
    }
}