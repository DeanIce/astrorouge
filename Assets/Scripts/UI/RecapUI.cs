using System.Collections;
using Managers;
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
        //private Label quip;
        private ScrollView scrollView;
        public AudioClip recapMusic;

        private void Start()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;
            AudioManager.Instance.PlayMusicWithCrossfade(recapMusic);

            //quip = root.Q<Label>("TerminalContent");
            //var (quote, _) = Quotes.Get();
            //quip.text += $"\"{quote}\"";

            scrollView = root.Q<ScrollView>("ScrollView");

            buttonMenu = root.Q<Button>("MenuButton");
            buttonExit = root.Q<Button>("ExitButton");
            buttonRetry = root.Q<Button>("RetryButton");
            buttonMenu.clicked += DoMenu;
            buttonExit.clicked += DoExit;
            buttonRetry.clicked += DoRetry;
            StartCoroutine(AddAllStats());
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.K)) AddStat("key", "value");
        }


        private WaitForSeconds AddStat(string key, object value)
        {
            var el = statDoc.Instantiate();
            el.Q<Label>("key").text = key;
            el.Q<Label>("value").text = value.ToString();
            scrollView.contentContainer.Add(el);
            return new WaitForSeconds(1);
        }

        private IEnumerator AddAllStats()
        {
            var runStats = EventManager.Instance.runStats;            
            yield return AddStat("Damage Dealt", Mathf.RoundToInt(runStats.damageDealt));
            yield return AddStat("Damage Taken", Mathf.RoundToInt(runStats.damageTaken));
            yield return AddStat("Enemies Killed", runStats.enemiesKilled);
            yield return AddStat("Items Collected", runStats.itemsCollected.Count);
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