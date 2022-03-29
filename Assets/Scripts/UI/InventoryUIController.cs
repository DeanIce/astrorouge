using System.Collections.Generic;
using Managers;
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

        private VisualElement tooltip;

        private void Start()
        {
            // set roots
            root = GetComponent<UIDocument>().rootVisualElement;
            inventoryContainer = root.Q<VisualElement>("inventory-container");
            tooltip = root.Q<VisualElement>("tooltip");
            tooltip.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
            RebuildInventory();
        }

        private void OnEnable()
        {
            EventManager.Instance.itemAcquired += EventResponse;
        }

        private void OnDisable()
        {
            EventManager.Instance.itemAcquired -= EventResponse;
        }

        public void RebuildInventory()
        {
            Dictionary<string, (AbstractItem, int)> inventory = EventManager.Instance.inventory;
            foreach (KeyValuePair<string, (AbstractItem, int)> pair in inventory)
            {
                (AbstractItem p, int i) tup = pair.Value;
                AbstractItem item = tup.p;
                AddItem(item, tup.i);
            }
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
            item.visualElement = temp;
            temp.RegisterCallback<MouseEnterEvent, AbstractItem>(MouseEntered, item);

            if (root == null || inventoryContainer == null || tooltip == null)
            {
                root = GetComponent<UIDocument>().rootVisualElement;
                inventoryContainer = root.Q<VisualElement>("inventory-container");
                tooltip = root.Q<VisualElement>("tooltip");
            }

            inventoryContainer.Add(temp);
        }

        private void MouseEntered(MouseEnterEvent ev, AbstractItem v)
        {
            v.visualElement.RegisterCallback<MouseMoveEvent>(MouseMoved);
            tooltip.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
            v.visualElement.RegisterCallback<MouseLeaveEvent>(MouseLeft);
            tooltip.Q<Label>("tooltip-name").text = v.itemName;
            tooltip.Q<Label>("tooltip-description").text = v.itemDescription;
        }

        private void MouseMoved(MouseMoveEvent ev)
        {
            tooltip.style.left = ev.mousePosition.x - tooltip.layout.width - 20;
            tooltip.style.top = ev.mousePosition.y - tooltip.layout.height - 20;
        }

        private void MouseLeft(MouseLeaveEvent ev)
        {
            tooltip.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
        }


        public void UpdateItem(AbstractItem item, int number)
        {
            Label boxLabel = inventoryContainer.Query<Label>(item.itemName).First();
            boxLabel.text = number.ToString();
        }

        private void EventResponse(AbstractItem item)
        {
            Dictionary<string, (AbstractItem, int)> inventory = EventManager.Instance.inventory;

            if (HasItem(item))
            {
                (AbstractItem p, int i) tup = inventory[item.itemName];
                inventory[item.itemName] = (tup.p, tup.i + 1);
                UpdateItem(item, inventory[item.itemName].Item2);
            }
            else
            {
                inventory.Add(item.itemName, (item, 1));
                AddItem(item, 1);
            }
        }

        public bool HasItem(AbstractItem item)
        {
            Dictionary<string, (AbstractItem, int)> inventory = EventManager.Instance.inventory;
            return inventory.ContainsKey(item.itemName);
        }
    }
}