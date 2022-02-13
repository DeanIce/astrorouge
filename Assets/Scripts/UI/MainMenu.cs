using Managers;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        public AudioClip mainMenuMusic;
        public AudioClip buttonPressSoundEffect;
        public float scrollSpeed = .5f;

        public float scrollAmount = 1000;

        private readonly string creditsText = @"
Created at The Ohio State University in the CSE 5912 Capstone.

<size=32>DANGER NOODLES TEAM</size>

Logan Flory
Matt Hall
Justin Holderby
Simon Kirksey
Sonja Linton
Dennis Miller
Jacob Woodhouse
Jared Zins

Professor Roger Crawfis


<size=32>TESTERS</size>

Todo: add tester names here



<size=32>ASSETS</size>

Todo: add assets here... names not links


2022

";

        private Button aboutBackButton;
        private Button aboutButton;
        private VisualElement aboutMenu;

        private Label creditsScroll;

        private VisualElement mainMenu;
        // private Slider musicSlider;
        // private float musicVolumeValue;
        // private Toggle muteButton;

        // private bool muteValue;

        private Button newGameButton;
        private Button quitButton;

        private bool scrolling;

        // private Button settingsBackButton;
        private Button settingsButton;

        private VisualElement settingsMenu;
        // private Slider sfxSlider;
        // private float sfxVolumeValue;

        // Start is called before the first frame update
        private void Start()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;

            settingsMenu = root.Q<VisualElement>("SettingsMenu");
            mainMenu = root.Q<VisualElement>("MainMenu");
            aboutMenu = root.Q<VisualElement>("AboutMenu");

            //Main Menu buttons
            newGameButton = mainMenu.Q<Button>("newgame-button");
            settingsButton = mainMenu.Q<Button>("settings-button");
            aboutButton = mainMenu.Q<Button>("about-button");
            quitButton = mainMenu.Q<Button>("quit-button");

            //Settings buttons
            // settingsBackButton = settingsMenu.Q<Button>("back-button");
            // muteButton = settingsMenu.Q<Toggle>("mute-button");
            // musicSlider = settingsMenu.Q<Slider>("music-volume-slider");
            // sfxSlider = settingsMenu.Q<Slider>("sfx-volume-slider");

            //About buttons
            aboutBackButton = aboutMenu.Q<Button>("back-button");
            creditsScroll = aboutMenu.Q<Label>("credits-scroll");
            creditsScroll.text = creditsText;

            quitButton.clicked += QuitButtonPressed;
            newGameButton.clicked += NewGameButtonPressed;
            settingsButton.clicked += SettingsButtonPressed;
            aboutButton.clicked += AboutButtonPressed;
            // settingsBackButton.clicked += BackButtonPressed;
            aboutBackButton.clicked += BackButtonPressed;

            // muteValue = muteButton.value;
            // musicVolumeValue = musicSlider.value;
            // sfxVolumeValue = sfxSlider.value;

            AudioManager.instance.PlayMusic(mainMenuMusic);
        }


        private void Update()
        {
            // if (muteValue != muteButton.value)
            // {
            //     muteValue = muteButton.value;
            //     AudioManager.instance.ToggleMute();
            // }
            //
            // if (musicVolumeValue != musicSlider.value)
            // {
            //     musicVolumeValue = musicSlider.value;
            //     AudioManager.instance.SetMusicVolume(musicVolumeValue);
            // }

            // if (sfxVolumeValue != sfxSlider.value)
            // {
            //     sfxVolumeValue = sfxSlider.value;
            //     AudioManager.instance.SetSFXVolume(sfxVolumeValue);
            // }

            // Cute hack to get scrolling credits
            // Todo: make credit scroll more resolution and aspect ratio independent
            var val = creditsScroll.style.marginTop.value.value;
            if (scrolling && val > -scrollAmount)
            {
                creditsScroll.style.marginTop = new StyleLength(val - scrollSpeed);
            }
            else if (scrolling && val <= -scrollAmount)
            {
                BackButtonPressed();
                scrolling = false;
            }
            else
            {
                creditsScroll.style.marginTop = new StyleLength(0f);
                scrolling = false;
            }
        }

        private void NewGameButtonPressed()
        {
            AudioManager.instance.PlaySFX(buttonPressSoundEffect);
            EventManager.instance.Play();
        }

        private void SettingsButtonPressed()
        {
            AudioManager.instance.PlaySFX(buttonPressSoundEffect);
            settingsMenu.style.display = DisplayStyle.Flex;
            mainMenu.style.display = DisplayStyle.None;
            aboutMenu.style.display = DisplayStyle.None;
        }

        private void AboutButtonPressed()
        {
            AudioManager.instance.PlaySFX(buttonPressSoundEffect);
            settingsMenu.style.display = DisplayStyle.None;
            mainMenu.style.display = DisplayStyle.None;
            aboutMenu.style.display = DisplayStyle.Flex;
            scrolling = true;
        }

        private void QuitButtonPressed()
        {
            AudioManager.instance.PlaySFX(buttonPressSoundEffect);
            EventManager.instance.Exit();
        }

        private void BackButtonPressed()
        {
            AudioManager.instance.PlaySFX(buttonPressSoundEffect);
            settingsMenu.style.display = DisplayStyle.None;
            mainMenu.style.display = DisplayStyle.Flex;
            aboutMenu.style.display = DisplayStyle.None;
            scrolling = false;
        }
    }
}