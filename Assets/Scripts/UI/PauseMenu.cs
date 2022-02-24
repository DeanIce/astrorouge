using Managers;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace UI
{
    public class PauseMenu : MonoBehaviour
    {
        public AudioClip mainMenuMusic;
        public AudioClip buttonPressSoundEffect;

        private Button continueButton;
        private Button mainMenuButton;

        private bool muteValue;
        private VisualElement pauseMenu;

        private VisualElement root;
        private Button settingsBackButton;
        private Button settingsButton;
        private VisualElement settingsMenu;

        private bool settingsOpen;

        // Start is called before the first frame update
        private void Start()
        {
            root = GetComponent<UIDocument>().rootVisualElement;

            settingsMenu = root.Q<VisualElement>("SettingsMenu");
            pauseMenu = root.Q<VisualElement>("PauseMenu");

            //Main Menu buttons
            continueButton = pauseMenu.Q<Button>("continue-button");
            settingsButton = pauseMenu.Q<Button>("settings-button");
            mainMenuButton = pauseMenu.Q<Button>("main-menu-button");

            //Settings buttons
            settingsBackButton = settingsMenu.Q<Button>("back-button");

            continueButton.clicked += ContinueButtonPressed;
            settingsButton.clicked += SettingsButtonPressed;
            settingsBackButton.clicked += BackButtonPressed;
            mainMenuButton.clicked += MainMenuButtonPressed;


            AudioManager.Instance.PlayMusic(mainMenuMusic);

            PlayGame();
        }


        private void OnEnable()
        {
            EventManager.Instance.pauseGame += PauseGame;
            EventManager.Instance.playGame += PlayGame;
            InputManager.inputActions.PauseMenu.Back.performed += GoBack;
        }

        private void OnDisable()
        {
            EventManager.Instance.pauseGame -= PauseGame;
            EventManager.Instance.playGame -= PlayGame;
            InputManager.inputActions.PauseMenu.Back.performed -= GoBack;
        }

        private void PauseGame()
        {
            root.SetEnabled(true);
            pauseMenu.style.display = DisplayStyle.Flex;
        }

        private void PlayGame()
        {
            pauseMenu.style.display = DisplayStyle.None;
            settingsMenu.style.display = DisplayStyle.None;
            root.SetEnabled(false);
        }

        private void GoBack(InputAction.CallbackContext obj)
        {
            if (settingsOpen)
            {
                BackButtonPressed();
            }
            else
            {
                settingsMenu.style.display = DisplayStyle.None;
                EventManager.Instance.Play();
            }
        }


        private void ContinueButtonPressed()
        {
            AudioManager.Instance.PlaySFX(buttonPressSoundEffect);
            EventManager.Instance.Play();
        }


        private void SettingsButtonPressed()
        {
            AudioManager.Instance.PlaySFX(buttonPressSoundEffect);
            settingsMenu.style.display = DisplayStyle.Flex;
            settingsOpen = true;
            print(settingsOpen);
        }

        private void MainMenuButtonPressed()
        {
            EventManager.Instance.Menu();
        }

        private void BackButtonPressed()
        {
            AudioManager.Instance.PlaySFX(buttonPressSoundEffect);
            settingsMenu.style.display = DisplayStyle.None;
            pauseMenu.style.display = DisplayStyle.Flex;
            settingsOpen = false;
        }
    }
}