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
    public float invincibilityDuration; // How long (seconds) player is immune to damage after getting hit

    // Movement Stats
    public int maxExtraJumps;
    public float jumpForce;
    public float extraJumpDampaner;
    public float movementSpeed;
    public float sprintMultiplier;
    public int dashCharges;
    public float dashDistance;
    public float dashRechargeRate;

    //
    // BASE STATS
    //

    // Base Melee Stats
    public float baseMeleeAttackDelay;
    public int baseMeleeBaseDamage;
    public float baseMeleeDamageMultiplier;
    public float baseMeleeCritChance;
    public float baseMeleeCritMultiplier;
    public float baseMeleeKnockbackForce;
    public float baseMeleeAttackRange;

    // Base Range Stats
    public float baseRangeAttackDelay;
    public int baseRangeBaseDamage;
    public float baseRangeDamageMultiplier;
    [Range(0.0f, 1.0f)] public float baseRangeCritChance;
    public float baseRangeCritMultiplier;
    public float baseRangeKnockbackForce;
    public float baseRangeProjectileRange;
    public float baseRangeProjectileSpeed;

    // Base Defense Stats
    public int baseMaxHealth;
    public float baseHealthRegen;
    public int baseArmor;
    public float baseDodgeChance;
    public float baseInvincibilityDuration;

    // Base Movement Stats
    public int baseMaxExtraJumps;
    public float baseJumpForce;
    public float baseExtraJumpDampaner;
    public float baseMovementSpeed;
    public float baseSprintMultiplier;
    public int baseDashCharges;
    public float baseDashDistance;
    public float baseDashRechargeRate;

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
        meleeAttackDelay = baseMeleeAttackDelay;
        meleeBaseDamage = baseMeleeBaseDamage;
        meleeDamageMultiplier = baseMeleeDamageMultiplier;
        meleeCritChance = baseMeleeCritChance;
        meleeCritMultiplier = baseMeleeCritMultiplier;
        meleeKnockbackForce = baseMeleeKnockbackForce;
        meleeAttackRange = baseMeleeAttackRange;

        // Range Stats
        rangeAttackDelay = baseRangeAttackDelay;
        rangeBaseDamage = baseRangeBaseDamage;
        rangeDamageMultiplier = baseRangeDamageMultiplier;
        rangeCritChance = baseRangeCritChance;
        rangeCritMultiplier = baseRangeCritMultiplier;
        rangeKnockbackForce = baseRangeKnockbackForce;
        rangeProjectileRange = baseRangeProjectileRange;
        rangeProjectileSpeed = baseRangeProjectileSpeed;

        // Defense Stats
        maxHealth = baseMaxHealth;
        currentHealth = maxHealth;
        healthRegen = baseHealthRegen;
        armor = baseArmor;
        dodgeChance = baseDodgeChance;
        invincibilityDuration = baseInvincibilityDuration;

        // Movement Stats
        maxExtraJumps = baseMaxExtraJumps;
        jumpForce = baseJumpForce;
        extraJumpDampaner = baseExtraJumpDampaner;
        movementSpeed = baseMovementSpeed;
        sprintMultiplier = baseSprintMultiplier;
        dashCharges = baseDashCharges;
        dashDistance = baseDashDistance;
        dashRechargeRate = baseDashRechargeRate;
    }
}