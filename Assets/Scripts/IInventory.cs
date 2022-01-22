using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IInventory : MonoBehaviour
{
   Dictionary<string, int> inventory = new Dictionary<string, int>();
   private InventoryUIController uic;

   void Start() {
       uic = GetComponent<InventoryUIController>();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.A)) {
            Add_Item("item_one");
        } 
        if (Input.GetKeyDown(KeyCode.B)) {
            Add_Item("dagger");
        }
    }
   
   public bool Has_Item(string item_name) {
       if (inventory.ContainsKey(item_name)) return true;
       else return false;
   }

   public void Add_Item(string item_name) {
       if (this.Has_Item(item_name)) {
           inventory[item_name] = inventory[item_name] + 1;
           uic.Update_Item(item_name, inventory[item_name]);
       } else {
           inventory.Add(item_name, 1);
           uic.Add_Item(item_name, 1);
       }
   }
}
