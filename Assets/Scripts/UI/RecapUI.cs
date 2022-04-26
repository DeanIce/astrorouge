using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class RecapUI : MonoBehaviour
    {
        public VisualTreeAsset statDoc;
        public AudioClip recapMusic;
        private Button buttonExit;
        private Button buttonMenu;

        private Button buttonRetry;

        private VisualElement deathCam;
        private VisualElement deathCamTexture;

        private ScrollView scrollViewItems;

        //private Label quip;
        private ScrollView scrollViewStats;

        private void Start()
        {
            VisualElement root = GetComponent<UIDocument>().rootVisualElement;
            AudioManager.Instance.PlayMusicWithCrossfade(recapMusic);

            //quip = root.Q<Label>("TerminalContent");
            //var (quote, _) = Quotes.Get();
            //quip.text += $"\"{quote}\"";

            scrollViewStats = root.Q<ScrollView>("ScrollView");
            scrollViewItems = root.Q<ScrollView>("ScrollViewItems");
            deathCam = root.Q<VisualElement>("DeathCam");
            deathCamTexture = root.Q<VisualElement>("DeathCamTexture");
            deathCamTexture.style.backgroundImage = new StyleBackground(EventManager.Instance.deathCam);

            buttonMenu = root.Q<Button>("MenuButton");
            buttonExit = root.Q<Button>("ExitButton");
            buttonRetry = root.Q<Button>("RetryButton");
            buttonMenu.clicked += DoMenu;
            buttonExit.clicked += DoExit;
            buttonRetry.clicked += DoRetry;
            StartCoroutine(AddAllStats());
            StartCoroutine(AddAllItems());
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.K)) AddStat("key", "value");
        }


        private WaitForSeconds AddStat(string key, object value)
        {
            TemplateContainer el = statDoc.Instantiate();
            el.Q<Label>("key").text = key;
            el.Q<Label>("value").text = value.ToString();
            scrollViewStats.contentContainer.Add(el);
            return new WaitForSeconds(1);
        }

        private IEnumerator AddAllStats()
        {
            RunStats runStats = EventManager.Instance.runStats;
            yield return AddStat("Damage Dealt", Mathf.RoundToInt(runStats.damageDealt));
            yield return AddStat("Damage Taken", Mathf.RoundToInt(runStats.damageTaken));
            yield return AddStat("Enemies Killed", runStats.enemiesKilled);
            yield return AddStat("Items Collected", runStats.itemsCollected.Count);
        }

        private WaitForSeconds AddItem(string key, object value)
        {
            TemplateContainer el = statDoc.Instantiate();
            el.Q<Label>("key").text = key;
            el.Q<Label>("value").text = value.ToString();
            scrollViewItems.contentContainer.Add(el);
            return new WaitForSeconds(1);
        }

        private IEnumerator AddAllItems()
        {
            RunStats runStats = EventManager.Instance.runStats;
            var dict = new Dictionary<string, int>();
            foreach (string s in runStats.itemsCollected)
            {
                if (dict.ContainsKey(s)) dict[s]++;
                else
                    dict[s] = 1;
            }

            foreach (KeyValuePair<string, int> keyValuePair in dict)
            {
                yield return AddItem(keyValuePair.Key, keyValuePair.Value);
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