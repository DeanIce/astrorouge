using System;
using System.Collections.Generic;

[Serializable]
public class PersistentUpgradesData 
{
    public int currency;
    public List<string> purchasedNodes;
    public List<string> statNames;
    public List<float> statValues;

    public PersistentUpgradesData()
    {
        currency = 0;
        purchasedNodes = new();
        statNames = new();
        statValues = new();
    }
}
