using Managers;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace UI
{
    public class PauseMenu : MonoBehaviour
    {
        public AudioClip buttonPressSoundEffect;
        public AudioClip openMenuSoundEffect;
        public AudioClip closeMenuSoundEffect;
        public AudioClip buttonHoverSoundEffect;

        private Button continueButton;
        private Button mainMenuButton;

        private bool muteValue;
        private VisualElement pauseMenu;

        private VisualElement root;
        private Button settingsBackButton;
        private Button settingsButton;
        private VisualElement settingsMenu;

        private bool settingsOpen;

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

            continueButton.RegisterCallback<MouseEnterEvent>(PlaySound);
            settingsButton.RegisterCallback<MouseEnterEvent>(PlaySound);
            mainMenuButton.RegisterCallback<MouseEnterEvent>(PlaySound);

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
            pauseMenu.SetEnabled(true);
            // settingsMenu.SetEnabled(true);
            pauseMenu.style.display = DisplayStyle.Flex;
            AudioManager.Instance.PlayMenuOpen();
        }

        private void PlayGame()
        {
            pauseMenu.style.display = DisplayStyle.None;
            settingsMenu.style.display = DisplayStyle.None;
            // root.SetEnabled(false);
            pauseMenu.SetEnabled(false);
        }

        private void GoBack(InputAction.CallbackContext obj)
        {
            if (settingsOpen)
                BackButtonPressed();
            else
                ContinueButtonPressed();
        }


        private void ContinueButtonPressed()
        {
            AudioManager.Instance.PlayMenuClose();
            EventManager.Instance.Play();
        }


        private void SettingsButtonPressed()
        {
            AudioManager.Instance.PlayMenuSelect();
            settingsMenu.style.display = DisplayStyle.Flex;
            settingsOpen = true;
        }

        private void MainMenuButtonPressed()
        {
            AudioManager.Instance.PlayMenuSelect();
            EventManager.Instance.Menu();
        }

        private void BackButtonPressed()
        {
            AudioManager.Instance.PlayMenuSelect();
            settingsMenu.style.display = DisplayStyle.None;
            pauseMenu.style.display = DisplayStyle.Flex;
            settingsOpen = false;
        }

        private void PlaySound(MouseEnterEvent ev)
        {
            AudioManager.Instance.PlayMenuHover();
        }
    }
}