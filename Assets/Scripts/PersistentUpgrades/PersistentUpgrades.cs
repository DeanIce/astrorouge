using System;
using System.Collections.Generic;

[Serializable]
public class PersistentUpgrades
{
    public int currency = 0;

    public List<string> purchasedNodes = new();
    public Dictionary<string, float> statUpgrades = defaultStatUpgrades;

    // Constant
    public static readonly List<string> intStats = new()
    {
        "meleeBaseDamage",
        "rangeBaseDamage",
        "maxHealth",
        "armor",
        "baseMaxExtraJumps",
    };
    private static readonly Dictionary<string, float> defaultStatUpgrades = new()
    {
        // Melee Stats
        { "meleeBaseDamage", 0f },
        { "meleeDamageMultiplier", 0f },
        { "meleeCritChance", 0f },
        { "meleeCritMultiplier", 0f },
        { "meleeAttackRange", 0f },

        // Range Stats
        { "rangeBaseDamage", 0f },
        { "rangeDamageMultiplier", 0f },
        { "rangeCritChance", 0f },
        { "rangeCritMultiplier", 0f },
        { "rangeProjectileRange", 0f },

        // Defense Stats
        { "maxHealth", 0f },
        { "healthRegen", 0f },
        { "regenDelay", 0f },
        { "armor", 0f },

        // Movement Stats
        { "baseMaxExtraJumps", 0f },
        { "baseMovementSpeed", 0f },
        { "baseSprintMultiplier", 0f },

        // Bullet Effect Chances
        { "baseEffectChance", 0f },
    };

    public void ApplyStats(PlayerStats target)
    {
        target.meleeBaseDamage += (int)statUpgrades["meleeBaseDamage"];
        target.meleeDamageMultiplier += statUpgrades["meleeDamageMultiplier"];
        target.meleeCritChance += statUpgrades["meleeCritChance"];
        target.meleeCritMultiplier += statUpgrades["meleeCritMultiplier"];
        target.meleeAttackRange += statUpgrades["meleeAttackRange"];

        target.rangeBaseDamage += (int)statUpgrades["rangeBaseDamage"];
        target.rangeDamageMultiplier += statUpgrades["rangeDamageMultiplier"];
        target.rangeCritChance += statUpgrades["rangeCritChance"];
        target.rangeCritMultiplier += statUpgrades["rangeCritMultiplier"];
        target.rangeProjectileRange += statUpgrades["rangeProjectileRange"];

        target.maxHealth += (int)statUpgrades["maxHealth"];
        target.healthRegen += statUpgrades["healthRegen"];
        target.regenDelay += statUpgrades["regenDelay"];
        target.armor += (int)statUpgrades["armor"];

        target.baseMaxExtraJumps += (int)statUpgrades["baseMaxExtraJumps"];
        target.baseMovementSpeed += statUpgrades["baseMovementSpeed"];
        target.baseSprintMultiplier += statUpgrades["baseSprintMultiplier"];

        target.burnChance += statUpgrades["baseEffectChance"];
        target.poisonChance += statUpgrades["baseEffectChance"];
        target.lightningChance += statUpgrades["baseEffectChance"];
        target.radioactiveChance += statUpgrades["baseEffectChance"];
        target.smiteChance += statUpgrades["baseEffectChance"];
        target.slowChance += statUpgrades["baseEffectChance"];
        target.stunChance += statUpgrades["baseEffectChance"];
        target.martyrdomChance += statUpgrades["baseEffectChance"];
        target.igniteChance += statUpgrades["baseEffectChance"];
    }
}
