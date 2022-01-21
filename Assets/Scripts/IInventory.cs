using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IInventory : MonoBehaviour
{
   Dictionary<string, int> inventory = new Dictionary<string, int>();
   private UIController uic;

   void Start() {
       uic = GetComponent<UIController>();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.A)) {
            AddItem("item_one");
        } 
        if (Input.GetKeyDown(KeyCode.B)) {
            AddItem("dagger");
        }
    }
   
   public bool HasItem(string item_name) {
       if (inventory.ContainsKey(item_name)) return true;
       else return false;
   }

   public void AddItem(string item_name) {
       if (this.HasItem(item_name)) {
           inventory[item_name] = inventory[item_name]++;
       } else {
           inventory.Add(item_name, 1);
       }
       uic.Add_Item(item_name);
   }
}
