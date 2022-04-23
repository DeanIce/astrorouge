using UnityEngine;

public class PersistentUpgradeNode : MonoBehaviour
{
    public int price;

    [SerializeField] private bool bought;
    [SerializeField] private string nodeIdentifier;
    [SerializeField] private string statName;
    [SerializeField] private float statValue;

    // Start is called before the first frame update
    void Start()
    {
        bought = PersistentUpgradeManager.Instance.NodePurchased(nodeIdentifier);
    }

    /// <summary>
    /// Attempts to purchase this upgrade.
    /// </summary>
    /// <returns>If the purchase was successful.</returns>
    private bool Purchase()
    {
        if (bought || !PersistentUpgradeManager.Instance.AddPersistentUpgrade(nodeIdentifier, statName, statValue, price))
            return false;

        bought = true;
        return true;
    }
}
