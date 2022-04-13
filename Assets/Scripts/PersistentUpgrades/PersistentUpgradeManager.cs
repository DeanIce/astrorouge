using Managers;
using System;

public class PersistentUpgradeManager : ManagerSingleton<PersistentUpgradeManager>
{
    private PersistentUpgrades upgrades;
    private bool unsavedChanges = false;

    // Constants
    private const string upgradeSaveFileName = "persistentUpgrades";

    // Start is called before the first frame update
    void Start()
    {
        upgrades = PersistentData.Load<PersistentUpgrades>(upgradeSaveFileName);
    }

    // Update is called once per frame
    void Update()
    {
        if (unsavedChanges)
        {
            PersistentData.Save(upgrades, upgradeSaveFileName);
            unsavedChanges = false;
        }
    }

    private void OnDestroy()
    {
        PersistentData.Save(upgrades, upgradeSaveFileName);
    }

    public void ApplyPersistentStats()
    {
        upgrades.ApplyStats(PlayerStats.Instance);
    }

    /// <returns>If adding the upgrade was successful.</returns>
    public bool AddPersistentUpgrade(string nodeName, string statName, float value, int cost)
    {
        if (!upgrades.statUpgrades.ContainsKey(statName))
            throw new Exception($"No persistent upgrade named: {statName}, valid names are: {upgrades.statUpgrades.Keys}");
        if (upgrades.purchasedNodes.Contains(nodeName))
            throw new Exception($"Node {nodeName} is already purchased!");
        if (PersistentUpgrades.intStats.Contains(statName)
            && Math.Abs(value - Math.Truncate(value)) >= 0.001f)
            throw new Exception($"Value: {value} added to an int stat");

        if (!DecCurrency(cost))
            return false;

        upgrades.purchasedNodes.Add(nodeName);
        upgrades.statUpgrades[statName] += value;
        unsavedChanges = true;

        return true;
    }

    public bool NodePurchased(string nodeName)
    {
        return upgrades.purchasedNodes.Contains(nodeName);
    }

    public void IncCurrency(int value)
    {
        upgrades.currency += value;
        unsavedChanges = true;
    }

    public int GetCurrency() => upgrades.currency;

    /// <returns>If the deduction was successful.</returns>
    private bool DecCurrency(int value)
    {
        if (upgrades.currency < value)
            return false;

        upgrades.currency -= value;
        unsavedChanges = true;

        return true;
    }
}
