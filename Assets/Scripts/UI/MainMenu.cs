using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        public AudioClip mainMenuMusic;
        public float scrollSpeed = .3f;

        public float scrollAmount = 5000;

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


<size=32>RESOURCES</size>

Sebastian Lague
StackOverflow


<size=32>ASSETS</size>

IGNITECODERS Simple Water Shader URP
Low Poly Monster Bundle
qq.d.y Lava Surface URP
SCI FI CHARACTERS MEGA PACK VOL 1
SCI FI CHARACTERS MEGA PACK VOL 2
Synty POLYGON Icons Pack
Synty POLYGON Particle FX Pack
Synty POLYGON Sci-Fi Worlds Pack
Synty POLYGON Sci-Fi Space Pack


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
        private Button tutorialButton;
        private Button upgradeButton;

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
            tutorialButton = mainMenu.Q<Button>("tutorial-button");
            quitButton = mainMenu.Q<Button>("quit-button");
            upgradeButton = mainMenu.Q<Button>("upgrades-button");

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
            tutorialButton.clicked += () => { SceneManager.LoadScene("Tutorial"); };
            upgradeButton.clicked += UpgradeButtonPressed;

            quitButton.RegisterCallback<MouseEnterEvent>(PlaySound);
            newGameButton.RegisterCallback<MouseEnterEvent>(PlaySound);
            settingsButton.RegisterCallback<MouseEnterEvent>(PlaySound);
            aboutButton.RegisterCallback<MouseEnterEvent>(PlaySound);
            feedbackButton.RegisterCallback<MouseEnterEvent>(PlaySound);
            tutorialButton.RegisterCallback<MouseEnterEvent>(PlaySound);


            feedbackButton.clicked += () => Application.OpenURL("https://forms.gle/gvcTaM3z1M9WdjEEA");


            AudioManager.Instance.PlayMusic(mainMenuMusic);

            // SettingsButtonPressed();
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
            AudioManager.Instance.PlayMenuSelect();
            AudioManager.Instance.FadeOut();
            EventManager.Instance.Play();
        }

        private void SettingsButtonPressed()
        {
            AudioManager.Instance.PlayMenuSelect();
            settingsMenu.style.display = DisplayStyle.Flex;
            mainMenu.style.display = DisplayStyle.None;
            aboutMenu.style.display = DisplayStyle.None;
        }

        private void AboutButtonPressed()
        {
            AudioManager.Instance.PlayMenuSelect();
            settingsMenu.style.display = DisplayStyle.None;
            mainMenu.style.display = DisplayStyle.None;
            aboutMenu.style.display = DisplayStyle.Flex;
            scrolling = true;
        }

        private void QuitButtonPressed()
        {
            AudioManager.Instance.PlayMenuSelect();
            EventManager.Instance.Exit();
        }

        private void BackButtonPressed()
        {
            AudioManager.Instance.PlayMenuSelect();
            settingsMenu.style.display = DisplayStyle.None;
            mainMenu.style.display = DisplayStyle.Flex;
            aboutMenu.style.display = DisplayStyle.None;
            scrolling = false;
        }

        private void UpgradeButtonPressed()
        {
            AudioManager.Instance.PlayMenuSelect();
            //AudioManager.Instance.FadeOut();
            EventManager.Instance.UpgradeMenu();
        }

        private void PlaySound(MouseEnterEvent ev)
        {
            AudioManager.Instance.PlayMenuHover();
        }
    }
}