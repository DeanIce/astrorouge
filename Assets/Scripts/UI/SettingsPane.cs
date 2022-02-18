using Managers;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    /// <summary>
    ///     Supports persistent settings changes.
    ///     Known issues: doesn't set audio variables until settings are saved.
    ///     Only music suffers from this issue, SFX works fine.
    ///     Most likely cause- event orders
    ///     Todo: fix audio setup issue
    /// </summary>
    public class SettingsPane : MonoBehaviour
    {
        public UserSettings settings;

        private readonly string saveFile = "userSettings";
        private DropdownMenu displayMode;
        private Slider fov;
        private Slider gameVolume;
        private Slider masterVolume;
        private Slider musicVolume;
        private DropdownMenu resolution;
        private VisualElement root;
        private Button saveSettings;
        private Slider screenShake;

        private void Start()
        {
            root = GetComponent<UIDocument>().rootVisualElement;
            fov = root.Q<Slider>("fov");
            screenShake = root.Q<Slider>("screen-shake");
            masterVolume = root.Q<Slider>("volume-master");
            musicVolume = root.Q<Slider>("volume-music");
            gameVolume = root.Q<Slider>("volume-game");
            saveSettings = root.Q<Button>("save-settings");

            settings = PersistentUpgrades.Load<UserSettings>(saveFile);

            // Register events to update the UserSettings class
            fov.RegisterCallback<ChangeEvent<float>>(e => settings.fov = e.newValue);
            gameVolume.RegisterCallback<ChangeEvent<float>>(e => settings.volumeGame = e.newValue);
            masterVolume.RegisterCallback<ChangeEvent<float>>(e => settings.volumeMaster = e.newValue);
            musicVolume.RegisterCallback<ChangeEvent<float>>(e => settings.volumeMusic = e.newValue);

            // Trigger an event when Save is pressed that AudioManager (and others) can subscribe to
            saveSettings.clicked += () =>
            {
                AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClick);
                EventManager.Instance.UpdateSettings(settings, saveFile);
            };

            // Set values when the panel is created
            Hydrate();
        }

        /// <summary>
        ///     "Hydrate" the settings UI with values from disk.
        ///     Add a line here to set the starting value of a control.
        /// </summary>
        private void Hydrate()
        {
            fov.value = settings.fov;
            gameVolume.value = settings.volumeGame;
            musicVolume.value = settings.volumeMusic;
            masterVolume.value = settings.volumeMaster;
            // silly hack to trigger settings events immediately
            EventManager.Instance.UpdateSettings(settings, saveFile);
        }
    }
}