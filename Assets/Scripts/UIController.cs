using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{

    public UIDocument inventoryHUD;
    VisualElement root;

    // Start is called before the first frame update
    void Start()
    {
        root = inventoryHUD.rootVisualElement;
        root.style.flexDirection = FlexDirection.Row;
        // root.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 100);
        // Add_Item(item_one);
        // Add_Item(item_two);
        // Add_Item(item_one);

    }

    public void Add_Item(string item_name)
    {
        print(item_name);
        var temp = new Button();
        temp.text = item_name;
        temp.style.width = 70;
        temp.style.height = 70;
        temp.style.marginBottom = 2;
        temp.style.backgroundColor = Color.gray;
        root.Add(temp);
    }
}
