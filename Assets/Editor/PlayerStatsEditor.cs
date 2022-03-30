using UnityEditor;

[CustomEditor(typeof(PlayerStats))]
[CanEditMultipleObjects]
public class PlayerStatsEditor : Editor
{
    private SerializedProperty armor;
    private SerializedProperty baseArmor;

    // Base Bullet Effect Chance
    private SerializedProperty baseBurnChance;
    private SerializedProperty baseDashCharges;
    private SerializedProperty baseDashDistance;
    private SerializedProperty baseDashRechargeRate;
    private SerializedProperty baseDodgeChance;
    private SerializedProperty baseExtraJumpDampaner;
    private SerializedProperty baseHealthRegen;
    private SerializedProperty baseIgniteChance;
    private SerializedProperty baseInvincibilityDuration;
    private SerializedProperty baseJumpForce;
    private SerializedProperty baseLightningChance;
    private SerializedProperty baseMartyrdomChance;

    // Base Movement Stats
    private SerializedProperty baseMaxExtraJumps;

    // Base Defense Stats
    private SerializedProperty baseMaxHealth;

    //
    // BASE STATS
    //

    // Base Melee Stats
    private SerializedProperty baseMeleeAttackDelay;
    private SerializedProperty baseMeleeAttackRange;
    private SerializedProperty baseMeleeBaseDamage;
    private SerializedProperty baseMeleeCritChance;
    private SerializedProperty baseMeleeCritMultiplier;
    private SerializedProperty baseMeleeDamageMultiplier;
    private SerializedProperty baseMeleeKnockbackForce;
    private SerializedProperty baseMovementSpeed;
    private SerializedProperty basePoisonChance;
    private SerializedProperty baseRadioactiveChance;

    // Base Range Stats
    private SerializedProperty baseRangeAttackDelay;
    private SerializedProperty baseRangeBaseDamage;
    private SerializedProperty baseRangeCritChance;
    private SerializedProperty baseRangeCritMultiplier;
    private SerializedProperty baseRangeDamageMultiplier;
    private SerializedProperty baseRangeKnockbackForce;
    private SerializedProperty baseRangeProjectileRange;
    private SerializedProperty baseRangeProjectileSpeed;
    private SerializedProperty baseRegenDelay;
    private SerializedProperty baseSlowChance;
    private SerializedProperty baseSmiteChance;
    private SerializedProperty baseSprintMultiplier;
    private SerializedProperty baseStunChance;

    // Bullet Effect Chances
    private SerializedProperty burnChance;
    private SerializedProperty currentHealth;
    private SerializedProperty dashCharges;
    private SerializedProperty dashDistance;
    private SerializedProperty dashRechargeRate;
    private SerializedProperty dodgeChance;
    private SerializedProperty extraJumpDampaner;
    private SerializedProperty healthRegen;
    private SerializedProperty igniteChance;
    private SerializedProperty invincibilityDuration;
    private SerializedProperty jumpForce;
    private SerializedProperty lightningChance;
    private SerializedProperty martyrdomChance;

    // Movement Stats
    private SerializedProperty maxExtraJumps;

    // Defense Stats
    private SerializedProperty maxHealth;

    // Melee Stats
    private SerializedProperty meleeAttackDelay;
    private SerializedProperty meleeAttackRange;
    private SerializedProperty meleeBaseDamage;
    private SerializedProperty meleeCritChance;
    private SerializedProperty meleeCritMultiplier;
    private SerializedProperty meleeDamageMultiplier;
    private SerializedProperty meleeKnockbackForce;
    private SerializedProperty movementSpeed;
    private SerializedProperty poisonChance;
    private SerializedProperty radioactiveChance;

    // Range Stats
    private SerializedProperty rangeAttackDelay;
    private SerializedProperty rangeBaseDamage;
    private SerializedProperty rangeCritChance;
    private SerializedProperty rangeCritMultiplier;
    private SerializedProperty rangeDamageMultiplier;
    private SerializedProperty rangeKnockbackForce;
    private SerializedProperty rangeProjectileRange;
    private SerializedProperty rangeProjectileSpeed;
    private SerializedProperty regenDelay;
    private bool showBaseDefense;
    private bool showBaseEffects;

    private bool showBaseMelee;
    private bool showBaseMovement;
    private bool showBaseRange;
    private bool showCurrentDefense;
    private bool showCurrentEffects;

    private bool showCurrentMelee;
    private bool showCurrentMovement;
    private bool showCurrentRange;
    private SerializedProperty slowChance;
    private SerializedProperty smiteChance;
    private SerializedProperty sprintMultiplier;
    private SerializedProperty stunChance;

    private void OnEnable()
    {
        meleeAttackDelay = serializedObject.FindProperty("meleeAttackDelay");
        meleeBaseDamage = serializedObject.FindProperty("meleeBaseDamage");
        meleeDamageMultiplier = serializedObject.FindProperty("meleeDamageMultiplier");
        meleeCritChance = serializedObject.FindProperty("meleeCritChance");
        meleeCritMultiplier = serializedObject.FindProperty("meleeCritMultiplier");
        meleeKnockbackForce = serializedObject.FindProperty("meleeKnockbackForce");
        meleeAttackRange = serializedObject.FindProperty("meleeAttackRange");

        // Range Stats
        rangeAttackDelay = serializedObject.FindProperty("rangeAttackDelay");
        rangeBaseDamage = serializedObject.FindProperty("rangeBaseDamage");
        rangeDamageMultiplier = serializedObject.FindProperty("rangeDamageMultiplier");
        rangeCritChance = serializedObject.FindProperty("rangeCritChance");
        rangeCritMultiplier = serializedObject.FindProperty("rangeCritMultiplier");
        rangeKnockbackForce = serializedObject.FindProperty("rangeKnockbackForce");
        rangeProjectileRange = serializedObject.FindProperty("rangeProjectileRange");
        rangeProjectileSpeed = serializedObject.FindProperty("rangeProjectileSpeed");

        // Defense Stats
        maxHealth = serializedObject.FindProperty("maxHealth");
        currentHealth = serializedObject.FindProperty("currentHealth");
        healthRegen = serializedObject.FindProperty("healthRegen");
        regenDelay = serializedObject.FindProperty("regenDelay");
        armor = serializedObject.FindProperty("armor");
        dodgeChance = serializedObject.FindProperty("dodgeChance");
        invincibilityDuration = serializedObject.FindProperty("invincibilityDuration");

        // Movement Stats
        maxExtraJumps = serializedObject.FindProperty("maxExtraJumps");
        jumpForce = serializedObject.FindProperty("jumpForce");
        extraJumpDampaner = serializedObject.FindProperty("extraJumpDampaner");
        movementSpeed = serializedObject.FindProperty("movementSpeed");
        sprintMultiplier = serializedObject.FindProperty("sprintMultiplier");
        dashCharges = serializedObject.FindProperty("dashCharges");
        dashDistance = serializedObject.FindProperty("dashDistance");
        dashRechargeRate = serializedObject.FindProperty("dashRechargeRate");

        // Bullet Effect Chances
        burnChance = serializedObject.FindProperty("burnChance");
        poisonChance = serializedObject.FindProperty("poisonChance");
        lightningChance = serializedObject.FindProperty("lightningChance");
        radioactiveChance = serializedObject.FindProperty("radioactiveChance");
        smiteChance = serializedObject.FindProperty("smiteChance");
        slowChance = serializedObject.FindProperty("slowChance");
        stunChance = serializedObject.FindProperty("stunChance");
        martyrdomChance = serializedObject.FindProperty("martyrdomChance");
        igniteChance = serializedObject.FindProperty("igniteChance");

        //
        // BASE STATS
        //

        // Base Melee Stats
        baseMeleeAttackDelay = serializedObject.FindProperty("baseMeleeAttackDelay");
        baseMeleeBaseDamage = serializedObject.FindProperty("baseMeleeBaseDamage");
        baseMeleeDamageMultiplier = serializedObject.FindProperty("baseMeleeDamageMultiplier");
        baseMeleeCritChance = serializedObject.FindProperty("baseMeleeCritChance");
        baseMeleeCritMultiplier = serializedObject.FindProperty("baseMeleeCritMultiplier");
        baseMeleeKnockbackForce = serializedObject.FindProperty("baseMeleeKnockbackForce");
        baseMeleeAttackRange = serializedObject.FindProperty("baseMeleeAttackRange");

        // Base Range Stats
        baseRangeAttackDelay = serializedObject.FindProperty("baseRangeAttackDelay");
        baseRangeBaseDamage = serializedObject.FindProperty("baseRangeBaseDamage");
        baseRangeDamageMultiplier = serializedObject.FindProperty("baseRangeDamageMultiplier");
        baseRangeCritChance = serializedObject.FindProperty("baseRangeCritChance");
        baseRangeCritMultiplier = serializedObject.FindProperty("baseRangeCritMultiplier");
        baseRangeKnockbackForce = serializedObject.FindProperty("baseRangeKnockbackForce");
        baseRangeProjectileRange = serializedObject.FindProperty("baseRangeProjectileRange");
        baseRangeProjectileSpeed = serializedObject.FindProperty("baseRangeProjectileSpeed");

        // Base Defense Stats
        baseMaxHealth = serializedObject.FindProperty("baseMaxHealth");
        baseHealthRegen = serializedObject.FindProperty("baseHealthRegen");
        baseRegenDelay = serializedObject.FindProperty("baseRegenDelay");
        baseArmor = serializedObject.FindProperty("baseArmor");
        baseDodgeChance = serializedObject.FindProperty("baseDodgeChance");
        baseInvincibilityDuration = serializedObject.FindProperty("baseInvincibilityDuration");

        // Base Movement Stats
        baseMaxExtraJumps = serializedObject.FindProperty("baseMaxExtraJumps");
        baseJumpForce = serializedObject.FindProperty("baseJumpForce");
        baseExtraJumpDampaner = serializedObject.FindProperty("baseExtraJumpDampaner");
        baseMovementSpeed = serializedObject.FindProperty("baseMovementSpeed");
        baseSprintMultiplier = serializedObject.FindProperty("baseSprintMultiplier");
        baseDashCharges = serializedObject.FindProperty("baseDashCharges");
        baseDashDistance = serializedObject.FindProperty("baseDashDistance");
        baseDashRechargeRate = serializedObject.FindProperty("baseDashRechargeRate");

        // Base Bullet Effect Chance
        baseBurnChance = serializedObject.FindProperty("baseBurnChance");
        basePoisonChance = serializedObject.FindProperty("basePoisonChance");
        baseLightningChance = serializedObject.FindProperty("baseLightningChance");
        baseRadioactiveChance = serializedObject.FindProperty("baseRadioactiveChance");
        baseSmiteChance = serializedObject.FindProperty("baseSmiteChance");
        baseSlowChance = serializedObject.FindProperty("baseSlowChance");
        baseStunChance = serializedObject.FindProperty("baseStunChance");
        baseMartyrdomChance = serializedObject.FindProperty("baseMartyrdomChance");
        baseIgniteChance = serializedObject.FindProperty("baseIgniteChance");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        showCurrentMelee = CreateFoldout(showCurrentMelee, "Current Melee Stats",
            meleeAttackDelay,
            meleeBaseDamage,
            meleeDamageMultiplier,
            meleeCritChance,
            meleeCritMultiplier,
            meleeKnockbackForce,
            meleeAttackRange);

        showBaseMelee = CreateFoldout(showBaseMelee, "Base Melee Stats",
            baseMeleeAttackDelay,
            baseMeleeBaseDamage,
            baseMeleeDamageMultiplier,
            baseMeleeCritChance,
            baseMeleeCritMultiplier,
            baseMeleeKnockbackForce,
            baseMeleeAttackRange);

        showCurrentRange = CreateFoldout(showCurrentRange, "Current Range Stats",
            rangeAttackDelay,
            rangeBaseDamage,
            rangeDamageMultiplier,
            rangeCritChance,
            rangeCritMultiplier,
            rangeKnockbackForce,
            rangeProjectileRange,
            rangeProjectileSpeed);

        showBaseRange = CreateFoldout(showBaseRange, "Base Range Stats",
            baseRangeAttackDelay,
            baseRangeBaseDamage,
            baseRangeDamageMultiplier,
            baseRangeCritChance,
            baseRangeCritMultiplier,
            baseRangeKnockbackForce,
            baseRangeProjectileRange,
            baseRangeProjectileSpeed);

        showCurrentDefense = CreateFoldout(showCurrentDefense, "Current Defense Stats",
            maxHealth,
            currentHealth,
            healthRegen,
            regenDelay,
            armor,
            dodgeChance,
            invincibilityDuration);

        showBaseDefense = CreateFoldout(showBaseDefense, "Base Defense Stats",
            baseMaxHealth,
            baseHealthRegen,
            baseRegenDelay,
            baseArmor,
            baseDodgeChance,
            baseInvincibilityDuration);

        showCurrentMovement = CreateFoldout(showCurrentMovement, "Current Movement Stats",
            maxExtraJumps,
            jumpForce,
            extraJumpDampaner,
            movementSpeed,
            sprintMultiplier,
            dashCharges,
            dashDistance,
            dashRechargeRate);

        showBaseMovement = CreateFoldout(showBaseMovement, "Base Movement Stats",
            baseMaxExtraJumps,
            baseJumpForce,
            baseExtraJumpDampaner,
            baseMovementSpeed,
            baseSprintMultiplier,
            baseDashCharges,
            baseDashDistance,
            baseDashRechargeRate);

        showCurrentEffects = CreateFoldout(showCurrentEffects, "Current Effects Stats",
            burnChance,
            poisonChance,
            lightningChance,
            radioactiveChance,
            smiteChance,
            slowChance,
            stunChance,
            martyrdomChance,
            igniteChance);

        showBaseEffects = CreateFoldout(showBaseEffects, "Base Effects Stats",
            baseBurnChance,
            basePoisonChance,
            baseLightningChance,
            baseRadioactiveChance,
            baseSmiteChance,
            baseSlowChance,
            baseStunChance,
            baseMartyrdomChance,
            baseIgniteChance);

        serializedObject.ApplyModifiedProperties();
    }

    private bool CreateFoldout(bool extended, string title, params SerializedProperty[] properties)
    {
        bool result = EditorGUILayout.Foldout(extended, title, true);
        if (result)
        {
            EditorGUI.indentLevel++;
            foreach (SerializedProperty prop in properties)
            {
                EditorGUILayout.PropertyField(prop);
            }

            EditorGUI.indentLevel--;
        }

        return result;
    }
}