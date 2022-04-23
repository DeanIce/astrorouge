using Managers;
using System;

public class PersistentUpgradeManager : ManagerSingleton<PersistentUpgradeManager>
{
    private PersistentUpgrades upgrades;
    private bool unsavedChanges = false;

    // Constants
    private const string upgradeSaveFileName = "persistentUpgradesData";

    // Start is called before the first frame update
    void Start()
    {
        upgrades = new();
        upgrades.data = PersistentData.Load<PersistentUpgradesData>(upgradeSaveFileName);
    }

    // Update is called once per frame
    void Update()
    {
        if (unsavedChanges)
        {
            PersistentData.Save(upgrades.data, upgradeSaveFileName);
            unsavedChanges = false;
        }
    }

    private void OnDestroy()
    {
        if (upgrades != null && upgrades.data != null)
            PersistentData.Save(upgrades.data, upgradeSaveFileName);
    }

    public void Reset()
    {
        upgrades = new();
        upgrades.data = new();
        unsavedChanges = true;
    }

    public void ApplyPersistentStats()
    {
        upgrades.ApplyStats(PlayerStats.Instance);
    }

    /// <returns>If adding the upgrade was successful.</returns>
    public bool AddPersistentUpgrade(string nodeName, string statName, float value, int cost)
    {
        if (!PersistentUpgrades.defaultStatUpgrades.ContainsKey(statName))
            throw new Exception($"No persistent upgrade named: {statName}, valid names are: {PersistentUpgrades.defaultStatUpgrades.Keys}");
        if (upgrades.PurchasedNodes.Contains(nodeName))
            throw new Exception($"Node {nodeName} is already purchased!");
        if (PersistentUpgrades.intStats.Contains(statName)
            && Math.Abs(value - Math.Truncate(value)) >= 0.001f)
            throw new Exception($"Value: {value} added to an int stat");

        if (!DecCurrency(cost))
            return false;

        upgrades.PurchasedNodes.Add(nodeName);
        upgrades.AddStatUpgrade(statName, value);
        unsavedChanges = true;

        return true;
    }

    public bool NodePurchased(string nodeName)
    {
        return upgrades.PurchasedNodes.Contains(nodeName);
    }

    public void IncCurrency(int value)
    {
        upgrades.Currency += value;
        unsavedChanges = true;
    }

    public int GetCurrency() => upgrades.Currency;

    /// <returns>If the deduction was successful.</returns>
    private bool DecCurrency(int value)
    {
        if (upgrades.Currency < value)
            return false;

        upgrades.Currency -= value;
        unsavedChanges = true;

        return true;
    }
}
