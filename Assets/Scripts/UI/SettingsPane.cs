using System;
using System.Collections.Generic;
using System.Linq;
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
        private Slider brightness;
        private DropdownField displayMode;
        private Slider gameVolume;
        private Slider masterVolume;
        private Slider musicVolume;
        private DropdownField resolution;
        private VisualElement root;
        private Button saveSettings;
        private Slider screenShake;


        private void Start()
        {
            settings = PersistentUpgrades.Load<UserSettings>(saveFile);
            root = GetComponent<UIDocument>().rootVisualElement;

            // Fullscreen, Windowed etc
            displayMode = root.Q<DropdownField>("display-mode");
            displayMode.choices = new List<string>(Enum.GetNames(typeof(FullScreenMode)));
            displayMode.RegisterValueChangedCallback(e => settings.displayMode = (FullScreenMode) displayMode.index);
            Screen.fullScreenMode = settings.displayMode;
            displayMode.index = displayMode.choices.IndexOf(settings.displayMode.ToString());

            // Resolution
            Resolution[] rez = Screen.resolutions.Distinct().ToArray();
            resolution = root.Q<DropdownField>("resolution");
            resolution.choices = rez.Select((i, d) => i.ToString())
                .ToList();
            resolution.index = Array.IndexOf(rez, settings.resolution);
            resolution.RegisterValueChangedCallback(e => { settings.resolution = rez[resolution.index]; });

            // Screen brightness override
            brightness = root.Q<Slider>("brightness");
            brightness.value = settings.brightness;
            brightness.RegisterValueChangedCallback(e => settings.brightness = e.newValue);


            screenShake = root.Q<Slider>("screen-shake");
            masterVolume = root.Q<Slider>("volume-master");
            musicVolume = root.Q<Slider>("volume-music");
            gameVolume = root.Q<Slider>("volume-game");
            saveSettings = root.Q<Button>("save-settings");

            ApplyCurrentSettings();

            // Register events to update the UserSettings class

            gameVolume.RegisterCallback<ChangeEvent<float>>(e => settings.volumeGame = e.newValue);
            masterVolume.RegisterCallback<ChangeEvent<float>>(e => settings.volumeMaster = e.newValue);
            musicVolume.RegisterCallback<ChangeEvent<float>>(e => settings.volumeMusic = e.newValue);

            // Trigger an event when Save is pressed that AudioManager (and others) can subscribe to
            saveSettings.clicked += () =>
            {
                AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClick);
                EventManager.Instance.UpdateSettings(settings, saveFile);

                // Update screen settings
                Screen.SetResolution(settings.resolution.width, settings.resolution.height, settings.displayMode);
            };

            // Set values when the panel is created
            Hydrate();
        }

        private void SetupEnumField(DropdownField dropdownField)
        {
        }

        private void ApplyCurrentSettings()
        {
        }


        /// <summary>
        ///     "Hydrate" the settings UI with values from disk.
        ///     Add a line here to set the starting value of a control.
        /// </summary>
        private void Hydrate()
        {
            gameVolume.value = settings.volumeGame;
            musicVolume.value = settings.volumeMusic;
            masterVolume.value = settings.volumeMaster;
            // silly hack to trigger settings events immediately
            EventManager.Instance.UpdateSettings(settings, saveFile);
        }
    }
}