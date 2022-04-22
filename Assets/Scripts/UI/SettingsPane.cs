﻿using System;
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

        private readonly string saveFile = "userSettings";
        private DropdownField displayMode;
        private Slider gameVolume;
        private DropdownField msaa;
        private Slider musicVolume;
        private DropdownField resolution;
        private VisualElement root;
        private Button saveSettings;
        private UniversalRenderPipelineAsset urp;


        private void Start()
        {
            settings = PersistentUpgrades.Load<UserSettings>(saveFile);
            urp = (UniversalRenderPipelineAsset) QualitySettings.renderPipeline;
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
            // brightness = root.Q<Slider>("brightness");
            // brightness.value = settings.brightness;
            // brightness.RegisterValueChangedCallback(e => settings.brightness = e.newValue);
            msaa = root.Q<DropdownField>("msaa");
            msaa.index = settings.msaa switch
            {
                1 => 0,
                2 => 1,
                4 => 2,
                8 => 3,
                _ => 0
            };
            msaa.RegisterValueChangedCallback(e =>
            {
                settings.msaa = e.newValue switch
                {
                    "Disabled" => 1,
                    "2x" => 2,
                    "4x" => 4,
                    "8x" => 8,
                    _ => 0
                };
            });
            
            // Audio settings
            musicVolume = root.Q<Slider>("volume-music");
            gameVolume = root.Q<Slider>("volume-game");
            gameVolume.RegisterCallback<ChangeEvent<float>>(e => settings.volumeGame = e.newValue);
            musicVolume.RegisterCallback<ChangeEvent<float>>(e => settings.volumeMusic = e.newValue);
            gameVolume.value = settings.volumeGame;
            musicVolume.value = settings.volumeMusic;


            saveSettings = root.Q<Button>("save-settings");

            ApplyCurrentSettings();

            // Register events to update the UserSettings class


            // Trigger an event when Save is pressed that AudioManager (and others) can subscribe to
            saveSettings.clicked += ApplyCurrentSettings;

        }


        private void ApplyCurrentSettings()
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClick);
            EventManager.Instance.UpdateSettings(settings, saveFile);

            // Update screen settings
            Screen.SetResolution(settings.resolution.width, settings.resolution.height, settings.displayMode);

            // Update quality settings
            urp.msaaSampleCount = settings.msaa;
        }


        /// <summary>
        ///     "Hydrate" the settings UI with values from disk.
        ///     Add a line here to set the starting value of a control.
        /// </summary>
        private void Hydrate()
        {
            gameVolume.value = settings.volumeGame;
            musicVolume.value = settings.volumeMusic;
            // silly hack to trigger settings events immediately
            EventManager.Instance.UpdateSettings(settings, saveFile);
        }
    }
}