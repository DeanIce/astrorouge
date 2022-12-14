using System.Collections.Generic;
using Managers;
using UI;
using UnityEngine;

namespace Player
{
    public class Inventory : MonoBehaviour
    {
        public GameObject inventoryUI;
        private InventoryUIController uic;


        private void Start()
        {
            uic = inventoryUI.GetComponent<InventoryUIController>();

            RebuildInventory();
        }


        private bool HasItem(AbstractItem item)
        {
            Dictionary<string, (AbstractItem, int)> inventory = EventManager.Instance.inventory;
            return inventory.ContainsKey(item.itemName);
        }

        public void AddItem(AbstractItem item)
        {
            Dictionary<string, (AbstractItem, int)> inventory = EventManager.Instance.inventory;

            if (HasItem(item))
            {
                (AbstractItem p, int i) tup = inventory[item.itemName];
                inventory[item.itemName] = (tup.p, tup.i + 1);
                uic.UpdateItem(item, inventory[item.itemName].Item2);
            }
            else
            {
                inventory.Add(item.itemName, (item, 1));
                uic.AddItem(item, 1);
            }
        }

        public int GetItemCount(AbstractItem item)
        {
            if (!HasItem(item)) return 0;

            return EventManager.Instance.inventory[item.itemName].Item2;
        }

        public void RebuildInventory()
        {
            Dictionary<string, (AbstractItem, int)> inventory = EventManager.Instance.inventory;
            foreach (KeyValuePair<string, (AbstractItem, int)> pair in inventory)
            {
                (AbstractItem p, int i) tup = pair.Value;
                AbstractItem item = tup.p;
                uic.AddItem(item, tup.i);
            }
        }
    }
}