using System.Collections;
using Managers;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class FlashScreenUI : MonoBehaviour
    {
        public float lifetime = 0.75f;
        public float maximumOpacity = 50f;
        private float timer;
        private bool isDisplayed;
        private VisualElement flashScreen;

        private void Start()
        {
            flashScreen = GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("FlashImage");
            timer = 0;
            isDisplayed = false;
        }

        private void OnEnable()
        {
            EventManager.Instance.playerDamaged += EventResponse;
        }

        private void OnDisable()
        {
            EventManager.Instance.playerDamaged -= EventResponse;
        }

        private void Update()
        {
            if (isDisplayed)
            {
                timer -= Time.deltaTime;
                if (timer <= 0)
                {

                }
            }
        }

        private void EventResponse()
        {
            flashScreen.style.opacity = new StyleFloat();
        }
    }
}