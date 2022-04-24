using System.Collections.Generic;

public class PersistentUpgrades
{
    public PersistentUpgradesData data;

    public int Currency
    { 
        get => data.currency; 
        set => data.currency = value; 
    }
    public List<string> PurchasedNodes => data.purchasedNodes;

    // Constant
    public static readonly List<string> intStats = new()
    {
        "meleeBaseDamage",
        "rangeBaseDamage",
        "maxHealth",
        "armor",
        "maxExtraJumps",
    };
    public static readonly Dictionary<string, float> defaultStatUpgrades = new()
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
        { "maxExtraJumps", 0f },
        { "movementSpeed", 0f },
        { "sprintMultiplier", 0f },

        // Bullet Effect Chances
        { "effectChance", 0f },
    };

    public void ApplyStats(PlayerStats target)
    {
        Dictionary<string, float> statUpgrades = defaultStatUpgrades;
        for (int i = 0; i < data.statNames.Count; i++)
            statUpgrades[data.statNames[i]] += data.statValues[i];

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

        target.maxExtraJumps += (int)statUpgrades["maxExtraJumps"];
        target.movementSpeed += statUpgrades["movementSpeed"];
        target.sprintMultiplier += statUpgrades["sprintMultiplier"];

        target.burnChance += statUpgrades["effectChance"];
        target.poisonChance += statUpgrades["effectChance"];
        target.lightningChance += statUpgrades["effectChance"];
        target.radioactiveChance += statUpgrades["effectChance"];
        target.smiteChance += statUpgrades["effectChance"];
        target.slowChance += statUpgrades["effectChance"];
        target.stunChance += statUpgrades["effectChance"];
        target.martyrdomChance += statUpgrades["effectChance"];
        target.igniteChance += statUpgrades["effectChance"];
    }

    public void AddStatUpgrade(string statName, float value)
    {
        for(int i = 0; i < data.statNames.Count; i++)
        {
            if (data.statNames[i].Equals(statName))
            {
                data.statValues[i] += value;
                return;
            }
        }
        data.statNames.Add(statName);
        data.statValues.Add(value);
    }
}
