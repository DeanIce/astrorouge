using System.Collections;
using Managers;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class FlashScreenUI : MonoBehaviour
    {
        public float lifetime = 0.75f;
        public float maximumOpacity = 0.5f;

        private float currentOpacity;
        private float timer;
        private VisualElement flashScreen;

        private void Start()
        {
            flashScreen = GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("FlashImage");
            timer = 0;
            currentOpacity = 0;
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
            if (timer > 0)
            {
                timer -= Time.deltaTime;
                if (timer <= 0) timer = 0;

                float displayOpacity = currentOpacity * (timer / lifetime);
                Debug.Log(displayOpacity);
                flashScreen.style.opacity = displayOpacity;
            }
        }

        private void EventResponse(float percentHealth)
        {
            currentOpacity = percentHealth * maximumOpacity;
            timer = lifetime;
        }

    }
}