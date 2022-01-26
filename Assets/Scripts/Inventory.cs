using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public GameObject inventoryUI;
    private readonly Dictionary<string, (IItem, int)> inventory = new();
    private InventoryUIController uic;


    private void Start()
    {
        uic = inventoryUI.GetComponent<InventoryUIController>();
    }


    public bool HasItem(IItem item)
    {
        return inventory.ContainsKey(item.itemName);
    }

    public void AddItem(IItem item)
    {
        if (HasItem(item))
        {
            (IItem p, int i) tup = inventory[item.itemName];
            inventory[item.itemName] = (tup.p, tup.i + 1);
            uic.UpdateItem(item, inventory[item.itemName].Item2);
        }
        else
        {
            inventory.Add(item.itemName, (item, 1));
            uic.AddItem(item, 1);
        }
    }
}