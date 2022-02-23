﻿using Managers;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class RecapUI : MonoBehaviour
    {
        public VisualTreeAsset statDoc;
        private Button buttonExit;
        private Button buttonMenu;
        private Button buttonRetry;
        private Label quip;
        private ScrollView scrollView;

        private void Start()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;

            quip = root.Q<Label>("TerminalContent");
            var (quote, _) = Quotes.Get();
            quip.text += $"\"{quote}\"";

            scrollView = root.Q<ScrollView>("ScrollView");

            buttonMenu = root.Q<Button>("MenuButton");
            buttonExit = root.Q<Button>("ExitButton");
            buttonRetry = root.Q<Button>("RetryButton");
            buttonMenu.clicked += DoMenu;
            buttonExit.clicked += DoExit;
            buttonRetry.clicked += DoRetry;
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

        private void DoMenu()
        {
            // Todo: some nice animations for the transition to main menu
            EventManager.Instance.Menu();
        }

        private void DoExit()
        {
            EventManager.Instance.Exit();
        }

        private void DoRetry()
        {
            // Todo: some nice animations for the transition to retry
            EventManager.Instance.Play();
        }
    }
}