using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class IItem {
    public StyleBackground item_icon;
    public string item_name;
    public IItem(string given_item_name, StyleBackground icon_src) {
        item_icon = icon_src;
        item_name = given_item_name;
    }
}

public class Inventory : MonoBehaviour
{
   Dictionary<IItem, int> inventory = new Dictionary<IItem, int>();
   private InventoryUIController uic;

   public GameObject inventoryui;

   // prototype items
   IItem item_one;
   IItem item_two;
   // this needs something better
   public Texture2D item_one_background_texture;
   public Texture2D item_two_background_texture;

   void Start() {
       uic = inventoryui.GetComponent<InventoryUIController>();
       item_one = new IItem("dagger", new StyleBackground(item_one_background_texture));
       item_two = new IItem("coin_pouch", new StyleBackground(item_two_background_texture));
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.V)) {
            Add_Item(item_one);
        } 
        if (Input.GetKeyDown(KeyCode.B)) {
            Add_Item(item_two);
        }
    }
   
   public bool Has_Item(IItem item) {
       if (inventory.ContainsKey(item)) return true;
       else return false;
   }

   public void Add_Item(IItem item) {
       if (this.Has_Item(item)) {
           inventory[item] = inventory[item] + 1;
           uic.Update_Item(item, inventory[item]);
       } else {
           inventory.Add(item, 1);
           uic.Add_Item(item, 1);
       }
   }
}
