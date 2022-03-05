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

        showCurrentMelee = EditorGUILayout.Foldout(showCurrentMelee, "Current Melee Stats", true);
        if (showCurrentMelee)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(meleeAttackDelay);
            EditorGUILayout.PropertyField(meleeBaseDamage);
            EditorGUILayout.PropertyField(meleeDamageMultiplier);
            EditorGUILayout.PropertyField(meleeCritChance);
            EditorGUILayout.PropertyField(meleeCritMultiplier);
            EditorGUILayout.PropertyField(meleeKnockbackForce);
            EditorGUILayout.PropertyField(meleeAttackRange);
            EditorGUI.indentLevel--;
        }

        showBaseMelee = EditorGUILayout.Foldout(showBaseMelee, "Base Melee Stats", true);
        if (showBaseMelee)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(baseMeleeAttackDelay);
            EditorGUILayout.PropertyField(baseMeleeBaseDamage);
            EditorGUILayout.PropertyField(baseMeleeDamageMultiplier);
            EditorGUILayout.PropertyField(baseMeleeCritChance);
            EditorGUILayout.PropertyField(baseMeleeCritMultiplier);
            EditorGUILayout.PropertyField(baseMeleeKnockbackForce);
            EditorGUILayout.PropertyField(baseMeleeAttackRange);
            EditorGUI.indentLevel--;
        }

        showCurrentRange = EditorGUILayout.Foldout(showCurrentRange, "Current Range Stats", true);
        if (showCurrentRange)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(rangeAttackDelay);
            EditorGUILayout.PropertyField(rangeBaseDamage);
            EditorGUILayout.PropertyField(rangeDamageMultiplier);
            EditorGUILayout.PropertyField(rangeCritChance);
            EditorGUILayout.PropertyField(rangeCritMultiplier);
            EditorGUILayout.PropertyField(rangeKnockbackForce);
            EditorGUILayout.PropertyField(rangeProjectileRange);
            EditorGUILayout.PropertyField(rangeProjectileSpeed);
            EditorGUI.indentLevel--;
        }

        showBaseRange = EditorGUILayout.Foldout(showBaseRange, "Base Range Stats", true);
        if (showBaseRange)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(baseRangeAttackDelay);
            EditorGUILayout.PropertyField(baseRangeBaseDamage);
            EditorGUILayout.PropertyField(baseRangeDamageMultiplier);
            EditorGUILayout.PropertyField(baseRangeCritChance);
            EditorGUILayout.PropertyField(baseRangeCritMultiplier);
            EditorGUILayout.PropertyField(baseRangeKnockbackForce);
            EditorGUILayout.PropertyField(baseRangeProjectileRange);
            EditorGUILayout.PropertyField(baseRangeProjectileSpeed);
            EditorGUI.indentLevel--;
        }

        showCurrentDefense = EditorGUILayout.Foldout(showCurrentDefense, "Current Defense Stats", true);
        if (showCurrentDefense)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(maxHealth);
            EditorGUILayout.PropertyField(currentHealth);
            EditorGUILayout.PropertyField(healthRegen);
            EditorGUILayout.PropertyField(armor);
            EditorGUILayout.PropertyField(dodgeChance);
            EditorGUILayout.PropertyField(invincibilityDuration);
            EditorGUI.indentLevel--;
        }

        showBaseDefense = EditorGUILayout.Foldout(showBaseDefense, "Base Defense Stats", true);
        if (showBaseDefense)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(baseMaxHealth);
            EditorGUILayout.PropertyField(baseHealthRegen);
            EditorGUILayout.PropertyField(baseArmor);
            EditorGUILayout.PropertyField(baseDodgeChance);
            EditorGUILayout.PropertyField(baseInvincibilityDuration);
            EditorGUI.indentLevel--;
        }

        showCurrentMovement = EditorGUILayout.Foldout(showCurrentMovement, "Current Movement Stats", true);
        if (showCurrentMovement)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(maxExtraJumps);
            EditorGUILayout.PropertyField(jumpForce);
            EditorGUILayout.PropertyField(extraJumpDampaner);
            EditorGUILayout.PropertyField(movementSpeed);
            EditorGUILayout.PropertyField(sprintMultiplier);
            EditorGUILayout.PropertyField(dashCharges);
            EditorGUILayout.PropertyField(dashDistance);
            EditorGUILayout.PropertyField(dashRechargeRate);
            EditorGUI.indentLevel--;
        }

        showBaseMovement = EditorGUILayout.Foldout(showBaseMovement, "Base Movement Stats", true);
        if (showBaseMovement)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(baseMaxExtraJumps);
            EditorGUILayout.PropertyField(baseJumpForce);
            EditorGUILayout.PropertyField(baseExtraJumpDampaner);
            EditorGUILayout.PropertyField(baseMovementSpeed);
            EditorGUILayout.PropertyField(baseSprintMultiplier);
            EditorGUILayout.PropertyField(baseDashCharges);
            EditorGUILayout.PropertyField(baseDashDistance);
            EditorGUILayout.PropertyField(baseDashRechargeRate);
            EditorGUI.indentLevel--;
        }

        serializedObject.ApplyModifiedProperties();
    }
}