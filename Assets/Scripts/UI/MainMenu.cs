using Managers;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        public AudioClip mainMenuMusic;
        public AudioClip buttonPressSoundEffect;
        public AudioClip buttonHoverSoundEffect;
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
        private Button feedbackButton;

        private VisualElement mainMenu;

        private Button newGameButton;
        private Button quitButton;

        private bool scrolling;

        private Button settingsBackButton;
        private Button settingsButton;

        private VisualElement settingsMenu;

        private void Start()
        {
            VisualElement root = GetComponent<UIDocument>().rootVisualElement;

            settingsMenu = root.Q<VisualElement>("SettingsMenu");
            mainMenu = root.Q<VisualElement>("MainMenu");
            aboutMenu = root.Q<VisualElement>("AboutMenu");

            //Main Menu buttons
            newGameButton = mainMenu.Q<Button>("newgame-button");
            settingsButton = mainMenu.Q<Button>("settings-button");
            aboutButton = mainMenu.Q<Button>("about-button");
            quitButton = mainMenu.Q<Button>("quit-button");

            feedbackButton = mainMenu.Q<Button>("feedback-button");

            //Settings buttons
            settingsBackButton = settingsMenu.Q<Button>("back-button");
            aboutBackButton = aboutMenu.Q<Button>("back-button");
            creditsScroll = aboutMenu.Q<Label>("credits-scroll");
            creditsScroll.text = creditsText;

            quitButton.clicked += QuitButtonPressed;
            newGameButton.clicked += NewGameButtonPressed;
            settingsButton.clicked += SettingsButtonPressed;
            aboutButton.clicked += AboutButtonPressed;
            settingsBackButton.clicked += BackButtonPressed;
            aboutBackButton.clicked += BackButtonPressed;

            quitButton.RegisterCallback<MouseEnterEvent>(PlaySound);
            newGameButton.RegisterCallback<MouseEnterEvent>(PlaySound);
            settingsButton.RegisterCallback<MouseEnterEvent>(PlaySound);
            aboutButton.RegisterCallback<MouseEnterEvent>(PlaySound);
            feedbackButton.RegisterCallback<MouseEnterEvent>(PlaySound);


            feedbackButton.clicked += () => Application.OpenURL("https://forms.gle/gvcTaM3z1M9WdjEEA");


            AudioManager.Instance.PlayMusic(mainMenuMusic);
        }


        private void Update()
        {
            // Cute hack to get scrolling credits
            // Todo: make credit scroll more resolution and aspect ratio independent
            float val = creditsScroll.style.marginTop.value.value;
            if (scrolling && val > -scrollAmount)
                creditsScroll.style.marginTop = new StyleLength(val - scrollSpeed);
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
            AudioManager.Instance.PlaySFX(buttonPressSoundEffect);
            AudioManager.Instance.FadeOut();
            EventManager.Instance.Play();
        }

        private void SettingsButtonPressed()
        {
            AudioManager.Instance.PlaySFX(buttonPressSoundEffect);
            settingsMenu.style.display = DisplayStyle.Flex;
            mainMenu.style.display = DisplayStyle.None;
            aboutMenu.style.display = DisplayStyle.None;
        }

        private void AboutButtonPressed()
        {
            AudioManager.Instance.PlaySFX(buttonPressSoundEffect);
            settingsMenu.style.display = DisplayStyle.None;
            mainMenu.style.display = DisplayStyle.None;
            aboutMenu.style.display = DisplayStyle.Flex;
            scrolling = true;
        }

        private void QuitButtonPressed()
        {
            AudioManager.Instance.PlaySFX(buttonPressSoundEffect);
            EventManager.Instance.Exit();
        }

        private void BackButtonPressed()
        {
            AudioManager.Instance.PlaySFX(buttonPressSoundEffect);
            settingsMenu.style.display = DisplayStyle.None;
            mainMenu.style.display = DisplayStyle.Flex;
            aboutMenu.style.display = DisplayStyle.None;
            scrolling = false;
        }

        private void PlaySound(MouseEnterEvent ev)
        {
            AudioManager.Instance.PlaySFX(buttonHoverSoundEffect);
        }
    }
}