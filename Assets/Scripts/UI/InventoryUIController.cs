using UnityEngine;
using UnityEngine.UIElements;

public class InventoryUIController : MonoBehaviour
{
    public VisualTreeAsset inventoryItem;

    // public int row_max_box_count = 10;
    private readonly int box_height = 70;

    // private readonly int box_margin = 2;
    // private int box_count;

    private readonly int box_width = 70;
    // private VisualElement currentBox;

    private VisualElement inventoryContainer;
    private VisualElement root;
    private int root_height;
    private int root_width;

    // Start is called before the first frame update
    private void Start()
    {
        // // set parameters
        // root_height = box_height + box_margin;
        // root_width = root_height * row_max_box_count;
        // box_width = box_height;

        // set roots
        root = GetComponent<UIDocument>().rootVisualElement;
        inventoryContainer = root.Q<VisualElement>("inventory-container");


        // inventoryContainer.style.width = root_width;
        // inventoryContainer.style.height = root_height;
        // MakeNewBox();
        // inventoryContainer.Add(currentBox);
    }

    // private void MakeNewBox()
    // {
    //     var box = new VisualElement();
    //     // box.style.backgroundColor = Color.yellow;
    //     box.style.flexDirection = FlexDirection.Row;
    //     box.style.width = root_width;
    //     box.style.height = root_height;
    //     box_count++;
    //     currentBox = box;
    // }

    private VisualElement MakeItemSlot(AbstractItem item, int number)
    {
        VisualElement temp = inventoryItem.Instantiate().contentContainer;
        temp.name = item.itemName;

        var temp_label = temp.Q<Label>("count");
        // temp_label.style.position = Position.Absolute;
        // temp_label.style.right = 0;
        // temp_label.style.bottom = 0;
        temp_label.text = number.ToString();
        temp_label.name = item.itemName;

        var icon = temp.Q<VisualElement>("icon");
        icon.style.backgroundImage = new StyleBackground(item.itemIcon);
        return temp;
    }

    public void AddItem(AbstractItem item, int number)
    {
        // if (currentBox.childCount % 10 == 0 && currentBox.childCount != 0)
        // {
        //     MakeNewBox();
        //     inventoryContainer.Add(currentBox);
        //     inventoryContainer.style.height = root_height * box_count;
        // }

        VisualElement temp = MakeItemSlot(item, number);
        inventoryContainer.Add(temp);
    }

    public void UpdateItem(AbstractItem item, int number)
    {
        Label boxLabel = inventoryContainer.Query<Label>(item.itemName).First();
        boxLabel.text = number.ToString();
    }
}