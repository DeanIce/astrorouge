using System;
using System.Collections.Generic;
using System.Linq;
using Managers;
using UnityEngine;
using UnityEngine.Rendering.Universal;
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
        private DropdownField displayMode;
        private Slider gameVolume;
        private Slider look;
        private DropdownField msaa;
        private Slider musicVolume;
        private DropdownField resolution;
        private VisualElement root;
        private Button saveSettings;
        private UniversalRenderPipelineAsset urp;


        private void Start()
        {
            settings = PersistentUpgrades.Load<UserSettings>(UserSettings.SAVE_FILE);
            urp = (UniversalRenderPipelineAsset) QualitySettings.renderPipeline;
            root = GetComponent<UIDocument>().rootVisualElement;

            // Fullscreen, Windowed etc
            displayMode = root.Q<DropdownField>("display-mode");
            displayMode.choices = new List<string>(Enum.GetNames(typeof(FullScreenMode)));
            displayMode.RegisterValueChangedCallback(e => settings.DisplayMode = (FullScreenMode) displayMode.index);
            Screen.fullScreenMode = settings.DisplayMode;
            displayMode.index = displayMode.choices.IndexOf(settings.DisplayMode.ToString());

            // Resolution
            Resolution[] rez = Screen.resolutions.Distinct().ToArray();
            resolution = root.Q<DropdownField>("resolution");
            resolution.choices = rez.Select((i, d) => i.ToString())
                .ToList();
            resolution.index = Array.IndexOf(rez, settings.resolution);
            resolution.RegisterValueChangedCallback(e => { settings.resolution = rez[resolution.index]; });

            // Renderer MSAA 
            msaa = root.Q<DropdownField>("msaa");
            msaa.index = (int) Math.Log(settings.msaa, 2);
            msaa.RegisterValueChangedCallback(e => settings.msaa = UserSettings.mapping.GetValueOrDefault(e.newValue));

            // Audio settings
            musicVolume = root.Q<Slider>("volume-music");
            gameVolume = root.Q<Slider>("volume-game");
            gameVolume.RegisterCallback<ChangeEvent<float>>(e => settings.volumeGame = e.newValue);
            musicVolume.RegisterCallback<ChangeEvent<float>>(e => settings.volumeMusic = e.newValue);
            gameVolume.value = settings.volumeGame;
            musicVolume.value = settings.volumeMusic;

            // Look sensitivity
            look = root.Q<Slider>("look");
            look.value = settings.lookSensitivity;
            look.RegisterValueChangedCallback(e => settings.lookSensitivity = e.newValue);


            saveSettings = root.Q<Button>("save-settings");

            ApplyCurrentSettings();


            // Trigger an event when Save is pressed that AudioManager (and others) can subscribe to
            saveSettings.clicked += ApplyCurrentSettings;
        }


        private void ApplyCurrentSettings()
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClick);
            EventManager.Instance.UpdateSettings(settings, UserSettings.SAVE_FILE);

            // Update screen settings
            Screen.SetResolution(settings.resolution.width, settings.resolution.height, settings.DisplayMode);

            // Update quality settings
            urp.msaaSampleCount = settings.msaa;

            // Sensitivity settings
            EventManager.Instance.user.lookSensitivity = settings.lookSensitivity;
        }
    }
}