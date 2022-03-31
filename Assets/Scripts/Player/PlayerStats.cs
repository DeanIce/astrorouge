using System;
using Managers;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerStats : ManagerSingleton<PlayerStats>
{
    // Unexposed properties
    public float xp;
    public int xpLevel;
    public float xpPerLevel = 100;


    // Melee Stats
    public float meleeAttackDelay;
    public int meleeBaseDamage;
    public float meleeDamageMultiplier;
    [Range(0.0f, 1.0f)] public float meleeCritChance;
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
    public float regenDelay;
    public int armor;
    [Range(0.0f, 1.0f)] public float dodgeChance;
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

    // Bullet Effect Chances
    [Range(0.0f, 1.0f)] public float burnChance;
    [Range(0.0f, 1.0f)] public float poisonChance;
    [Range(0.0f, 1.0f)] public float lightningChance;
    [Range(0.0f, 1.0f)] public float radioactiveChance;
    [Range(0.0f, 1.0f)] public float smiteChance;
    [Range(0.0f, 1.0f)] public float slowChance;
    [Range(0.0f, 1.0f)] public float stunChance;
    [Range(0.0f, 1.0f)] public float martyrdomChance;
    [Range(0.0f, 1.0f)] public float igniteChance;

    //
    // BASE STATS
    //

    // Base Melee Stats
    public float baseMeleeAttackDelay;
    public int baseMeleeBaseDamage;
    public float baseMeleeDamageMultiplier;
    [Range(0.0f, 1.0f)] public float baseMeleeCritChance;
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
    public float baseRegenDelay;
    public int baseArmor;
    [Range(0.0f, 1.0f)] public float baseDodgeChance;
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

    // Base Bullet Effect Chance
    [Range(0.0f, 1.0f)] public float baseBurnChance;
    [Range(0.0f, 1.0f)] public float basePoisonChance;
    [Range(0.0f, 1.0f)] public float baseLightningChance;
    [Range(0.0f, 1.0f)] public float baseRadioactiveChance;
    [Range(0.0f, 1.0f)] public float baseSmiteChance;
    [Range(0.0f, 1.0f)] public float baseSlowChance;
    [Range(0.0f, 1.0f)] public float baseStunChance;
    [Range(0.0f, 1.0f)] public float baseMartyrdomChance;
    [Range(0.0f, 1.0f)] public float baseIgniteChance;


    private void Start()
    {
        SetDefaultValues();
    }


    public event Action MoustacheEnable;

    public float GetRangeDamage()
    {
        if (Random.value <= rangeCritChance)
            return rangeBaseDamage * rangeDamageMultiplier * rangeCritMultiplier;
        return rangeBaseDamage * rangeDamageMultiplier;
    }

    public bool IsAlive()
    {
        return currentHealth > 0 && currentHealth < maxHealth;
    }

    public void SetDefaultValues()
    {
        // Unexposed properties
        xpLevel = 0;
        xp = 0;

        // Melee Stats
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
        regenDelay = baseRegenDelay;
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

        // Bulllet Effect Stats
        burnChance = baseBurnChance;
        poisonChance = basePoisonChance;
        lightningChance = baseLightningChance;
        radioactiveChance = baseRadioactiveChance;
        smiteChance = baseSmiteChance;
        slowChance = baseSlowChance;
        stunChance = baseStunChance;
        martyrdomChance = baseMartyrdomChance;
        igniteChance = baseIgniteChance;
    }

    protected internal void Moustache()
    {
        MoustacheEnable?.Invoke();
    }

    public void LevelUp()
    {
        maxHealth = (int) (maxHealth * 1.2f);
        meleeBaseDamage = (int) (meleeBaseDamage * 1.2f);
        rangeBaseDamage = (int) (rangeBaseDamage * 1.2f);
    }
}