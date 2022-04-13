using UnityEngine;

public class PersistentUpgradeManager : MonoBehaviour
{
    private PersistentUpgrades upgrades;
    private bool unsavedChanges = false;

    // Constants
    private const string upgradeSaveFileName = "persistentUpgrades";

    // Start is called before the first frame update
    void Start()
    {
        upgrades = Managers.PersistentData.Load<PersistentUpgrades>(upgradeSaveFileName);
    }

    // Update is called once per frame
    void Update()
    {
        if (unsavedChanges)
        {
            Managers.PersistentData.Save(upgrades, upgradeSaveFileName);
            unsavedChanges = false;
        }
    }

    public void IncCurrency(int value)
    {
        upgrades.currency += value;
        unsavedChanges = true;
    }

    /// <returns>If the deduction was successful.</returns>
    public bool DecCurrency(int value)
    {
        if (upgrades.currency < value)
            return false;

        upgrades.currency -= value;
        unsavedChanges = true;

        return true;
    }
}
