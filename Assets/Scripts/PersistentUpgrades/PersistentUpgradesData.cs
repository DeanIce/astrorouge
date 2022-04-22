using System;
using System.Collections.Generic;

[Serializable]
public class PersistentUpgradesData 
{
    public int currency;
    public List<string> purchasedNodes;
    public List<KeyValuePair<string, float>> statUpgrades;

    public PersistentUpgradesData()
    {
        currency = 0;
        purchasedNodes = new();
        statUpgrades = new();
    }
}
