using UnityEngine;
using UnityEngine.UIElements;

public class InventoryUIController : MonoBehaviour
{
    public UIDocument inventoryHUD;
    private readonly int box_height = 70;
    private readonly int box_margin = 2;
    private readonly int row_max_box_count = 10;
    private int box_count;
    private int box_width;
    private VisualElement currentBox;
    private VisualElement root;
    private int root_height;
    private int root_width;

    // Start is called before the first frame update
    private void Start()
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

    private void MakeNewBox()
    {
        var box = new VisualElement();
        // box.style.backgroundColor = Color.yellow;
        box.style.flexDirection = FlexDirection.Row;
        box.style.width = root_width;
        box.style.height = root_height;
        box_count++;
        currentBox = box;
    }

    private VisualElement MakeItemSlot(IItem item, int number)
    {
        var temp = new VisualElement();
        temp.style.width = box_width;
        temp.style.height = box_height;
        temp.style.marginRight = box_margin;
        temp.style.marginBottom = box_margin;
        temp.style.backgroundColor = Color.gray;
        temp.style.backgroundImage = new StyleBackground(item.itemIcon);
        temp.style.position = Position.Relative;

        var temp_label = new Label();
        temp_label.style.position = Position.Absolute;
        temp_label.style.right = 0;
        temp_label.style.bottom = 0;
        temp_label.text = number.ToString();
        temp_label.name = item.itemName;
        temp.Add(temp_label);
        return temp;
    }

    public void AddItem(IItem item, int number)
    {
        if (currentBox.childCount % 10 == 0 && currentBox.childCount != 0)
        {
            MakeNewBox();
            root.Add(currentBox);
            root.style.height = root_height * box_count;
        }

        var temp = MakeItemSlot(item, number);
        currentBox.Add(temp);
    }

    public void UpdateItem(IItem item, int number)
    {
        var boxLabel = root.Query<Label>(item.itemName).First();
        boxLabel.text = number.ToString();
    }
}