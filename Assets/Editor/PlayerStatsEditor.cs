using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerStats))]
[CanEditMultipleObjects]
public class PlayerStatsEditor : Editor
{
    // Melee Stats
    SerializedProperty meleeAttackDelay;
    SerializedProperty meleeBaseDamage;
    SerializedProperty meleeDamageMultiplier;
    SerializedProperty meleeCritChance;
    SerializedProperty meleeCritMultiplier;
    SerializedProperty meleeKnockbackForce;
    SerializedProperty meleeAttackRange;

    // Range Stats
    SerializedProperty rangeAttackDelay;
    SerializedProperty rangeBaseDamage;
    SerializedProperty rangeDamageMultiplier;
    SerializedProperty rangeCritChance;
    SerializedProperty rangeCritMultiplier;
    SerializedProperty rangeKnockbackForce;
    SerializedProperty rangeProjectileRange;
    SerializedProperty rangeProjectileSpeed;

    // Defense Stats
    SerializedProperty maxHealth;
    SerializedProperty currentHealth;
    SerializedProperty healthRegen;
    SerializedProperty armor;
    SerializedProperty dodgeChance;
    SerializedProperty invincibilityDuration;

    // Movement Stats
    SerializedProperty maxExtraJumps;
    SerializedProperty jumpForce;
    SerializedProperty extraJumpDampaner;
    SerializedProperty movementSpeed;
    SerializedProperty sprintMultiplier;
    SerializedProperty dashCharges;
    SerializedProperty dashDistance;
    SerializedProperty dashRechargeRate;

    //
    // BASE STATS
    //

    // Base Melee Stats
    SerializedProperty baseMeleeAttackDelay;
    SerializedProperty baseMeleeBaseDamage;
    SerializedProperty baseMeleeDamageMultiplier;
    SerializedProperty baseMeleeCritChance;
    SerializedProperty baseMeleeCritMultiplier;
    SerializedProperty baseMeleeKnockbackForce;
    SerializedProperty baseMeleeAttackRange;

    // Base Range Stats
    SerializedProperty baseRangeAttackDelay;
    SerializedProperty baseRangeBaseDamage;
    SerializedProperty baseRangeDamageMultiplier;
    SerializedProperty baseRangeCritChance;
    SerializedProperty baseRangeCritMultiplier;
    SerializedProperty baseRangeKnockbackForce;
    SerializedProperty baseRangeProjectileRange;
    SerializedProperty baseRangeProjectileSpeed;

    // Base Defense Stats
    SerializedProperty baseMaxHealth;
    SerializedProperty baseHealthRegen;
    SerializedProperty baseArmor;
    SerializedProperty baseDodgeChance;
    SerializedProperty baseInvincibilityDuration;

    // Base Movement Stats
    SerializedProperty baseMaxExtraJumps;
    SerializedProperty baseJumpForce;
    SerializedProperty baseExtraJumpDampaner;
    SerializedProperty baseMovementSpeed;
    SerializedProperty baseSprintMultiplier;
    SerializedProperty baseDashCharges;
    SerializedProperty baseDashDistance;
    SerializedProperty baseDashRechargeRate;

    private bool showCurrentMelee = false;
    private bool showCurrentRange = false;
    private bool showCurrentDefense = false;
    private bool showCurrentMovement = false;

    private bool showBaseMelee = false;
    private bool showBaseRange = false;
    private bool showBaseDefense = false;
    private bool showBaseMovement = false;

    void OnEnable()
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
            armor,
            dodgeChance,
            invincibilityDuration);

        showBaseDefense = CreateFoldout(showBaseDefense, "Base Defense Stats",
            baseMaxHealth,
            baseHealthRegen,
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

        serializedObject.ApplyModifiedProperties();
    }

    private bool CreateFoldout(bool extended, string title, params SerializedProperty[] properties)
    {
        bool result = EditorGUILayout.Foldout(extended, title, true);
        if (result)
        {
            EditorGUI.indentLevel++;
            foreach(SerializedProperty prop in properties)
            {
                EditorGUILayout.PropertyField(prop);
            }
            EditorGUI.indentLevel--;
        }
        return result;
    }
}