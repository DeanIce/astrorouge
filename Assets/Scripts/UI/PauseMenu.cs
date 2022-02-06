using Managers;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PauseMenu : MonoBehaviour
{
    public AudioClip mainMenuMusic;
    public AudioClip buttonPressSoundEffect;

    private Button continueButton;
    private Button mainMenuButton;
    private Slider musicSlider;
    private float musicVolumeValue;
    private Toggle muteButton;

    private bool muteValue;
    private VisualElement pauseMenu;

    private VisualElement root;
    private Button settingsBackButton;
    private Button settingsButton;
    private VisualElement settingsMenu;
    private Slider sfxSlider;
    private float sfxVolumeValue;

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
        muteButton = settingsMenu.Q<Toggle>("mute-button");
        musicSlider = settingsMenu.Q<Slider>("music-volume-slider");
        sfxSlider = settingsMenu.Q<Slider>("sfx-volume-slider");

        continueButton.clicked += ContinueButtonPressed;
        settingsButton.clicked += SettingsButtonPressed;
        settingsBackButton.clicked += BackButtonPressed;
        mainMenuButton.clicked += MainMenuButtonPressed;

        muteValue = muteButton.value;
        musicVolumeValue = musicSlider.value;
        sfxVolumeValue = sfxSlider.value;

        AudioManager.Instance.PlayMusic(mainMenuMusic);
    }


    private void Update()
    {
        if (muteValue != muteButton.value)
        {
            muteValue = muteButton.value;
            AudioManager.Instance.ToggleMute();
        }

        if (musicVolumeValue != musicSlider.value)
        {
            musicVolumeValue = musicSlider.value;
            AudioManager.Instance.SetMusicVolume(musicVolumeValue);
        }

        if (sfxVolumeValue != sfxSlider.value)
        {
            sfxVolumeValue = sfxSlider.value;
            AudioManager.Instance.SetSFXVolume(sfxVolumeValue);
        }
    }

    private void OnEnable()
    {
        EventManager.instance.pauseGame += PauseGame;
        EventManager.instance.playGame += PlayGame;
        InputManager.inputActions.PauseMenu.Back.performed += PlayGame;
    }

    private void OnDisable()
    {
        EventManager.instance.pauseGame -= PauseGame;
        EventManager.instance.playGame -= PlayGame;
        InputManager.inputActions.PauseMenu.Back.performed -= PlayGame;
    }

    private void PauseGame()
    {
        root.SetEnabled(true);
        pauseMenu.style.display = DisplayStyle.Flex;
    }

    private void PlayGame()
    {
        pauseMenu.style.display = DisplayStyle.None;
        root.SetEnabled(false);
    }

    private void PlayGame(InputAction.CallbackContext obj)
    {
        settingsMenu.style.display = DisplayStyle.None;
        EventManager.instance.Play();
    }


    private void ContinueButtonPressed()
    {
        AudioManager.Instance.PlaySFX(buttonPressSoundEffect);
        EventManager.instance.Play();
    }


    private void SettingsButtonPressed()
    {
        AudioManager.Instance.PlaySFX(buttonPressSoundEffect);
        settingsMenu.style.display = DisplayStyle.Flex;
        pauseMenu.style.display = DisplayStyle.None;
    }

    private void MainMenuButtonPressed()
    {
        EventManager.instance.Menu();
    }

    private void BackButtonPressed()
    {
        AudioManager.Instance.PlaySFX(buttonPressSoundEffect);
        settingsMenu.style.display = DisplayStyle.None;
        pauseMenu.style.display = DisplayStyle.Flex;
    }
}