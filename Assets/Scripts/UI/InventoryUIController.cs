using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class InventoryUIController : MonoBehaviour
    {
        public VisualTreeAsset inventoryItem;


        private VisualElement inventoryContainer;
        private VisualElement root;
        private int root_height;
        private int root_width;

        private void Start()
        {
            // set roots
            root = GetComponent<UIDocument>().rootVisualElement;
            inventoryContainer = root.Q<VisualElement>("inventory-container");
        }


        private VisualElement MakeItemSlot(AbstractItem item, int number)
        {
            VisualElement temp = inventoryItem.Instantiate().contentContainer;
            temp.name = item.itemName;

            var temp_label = temp.Q<Label>("count");

            temp_label.text = number.ToString();
            temp_label.name = item.itemName;

            var icon = temp.Q<VisualElement>("icon");
            icon.style.backgroundImage = new StyleBackground(item.itemIcon);
            return temp;
        }

        public void AddItem(AbstractItem item, int number)
        {
            VisualElement temp = MakeItemSlot(item, number);
            inventoryContainer.Add(temp);
        }

        public void UpdateItem(AbstractItem item, int number)
        {
            Label boxLabel = inventoryContainer.Query<Label>(item.itemName).First();
            boxLabel.text = number.ToString();
        }
    }
}