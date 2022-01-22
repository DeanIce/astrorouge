using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class InventoryUIController : MonoBehaviour
{

    public UIDocument inventoryHUD;
    VisualElement root;
    VisualElement currentBox;
    int row_max_box_count = 10;
    int box_height = 70;
    int box_width;
    int box_margin = 2;
    int root_height;
    int root_width;
    int box_count = 0;

    // Start is called before the first frame update
    void Start()
    {
        // set parameters
        root_height = box_height + box_margin;
        root_width = root_height * row_max_box_count;
        box_width = box_height;

        // set roots
        root = inventoryHUD.rootVisualElement;
        root.style.width = root_width;
        root.style.height = root_height;
        MakeNewBox();
        root.Add(currentBox);
    }

    private void MakeNewBox() {
        var box = new VisualElement();
        // box.style.backgroundColor = Color.yellow;
        box.style.flexDirection = FlexDirection.Row;
        box.style.width = root_width;
        box.style.height = root_height;
        box_count++;
        currentBox = box;
    }

    private VisualElement MakeItemSlot(string item_name, int number) {
        var temp = new VisualElement();
        temp.style.width = box_width;
        temp.style.height = box_height;
        temp.style.marginRight = box_margin;
        temp.style.marginBottom = box_margin;
        temp.style.backgroundColor = Color.gray;
        temp.style.position = Position.Relative;

        var temp_label = new Label();
        temp_label.style.position = Position.Absolute;
        temp_label.style.right = 0;
        temp_label.style.bottom = 0;
        temp_label.text = number.ToString();
        temp_label.name = item_name;
        temp.Add(temp_label);
        return temp;
    }

    public void Add_Item(string item_name, int number)
    {
        if (currentBox.childCount % 10 == 0 && currentBox.childCount != 0) {
            MakeNewBox();
            root.Add(currentBox);
            root.style.height = root_height * box_count;
        }
        var temp = MakeItemSlot(item_name, number);
        currentBox.Add(temp);
    }

    public void Update_Item(string item_name, int number) {
        Label box_label = root.Query<Label>(item_name).First();
        box_label.text = number.ToString();
    }
}
