using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class RecapUI : MonoBehaviour
    {
        public VisualTreeAsset statDoc;
        private Label quip;
        private ScrollView scrollView;

        private void Start()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;

            quip = root.Q<Label>("quip");
            var (quote, source) = Quotes.Get();
            quip.text = $"\"{quote}\"";

            scrollView = root.Q<ScrollView>("ScrollView");
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                var el = statDoc.Instantiate();
                el.Q<Label>("key").text = "Key";
                el.Q<Label>("value").text = "Value";
                scrollView.contentContainer.Add(el);
            }
        }
    }
}