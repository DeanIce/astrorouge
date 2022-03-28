using System.Collections;
using Managers;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class NotificationsUI : MonoBehaviour
    {
        public VisualTreeAsset notification;
        public float lifetime = 10;
        private VisualElement display;

        private void Start()
        {
            display = GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("display");
        }

        private void OnEnable()
        {
            EventManager.Instance.itemAcquired += EventResponse;
        }

        private void OnDisable()
        {
            EventManager.Instance.itemAcquired -= EventResponse;
        }

        private void EventResponse(AbstractItem item)
        {
            TemplateContainer el = notification.Instantiate();
            el.Q<Label>("itemName").text = item.itemName;
            el.Q<Label>("itemDescription").text = item.itemDescription;
            el.Q<VisualElement>("itemIcon").style.backgroundImage = new StyleBackground(item.itemIcon);

            display.contentContainer.Add(el);
            StartCoroutine(DeleteAfterDelay(el.contentContainer));
        }

        private IEnumerator DeleteAfterDelay(VisualElement el)
        {
            yield return new WaitForSeconds(lifetime);
            display.contentContainer.Remove(el);
        }
    }
}