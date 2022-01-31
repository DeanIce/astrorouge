using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PauseMenu : MonoBehaviour
{
    public delegate void Resume();

    public AudioClip mainMenuMusic;
    public AudioClip buttonPressSoundEffect;

    public Button continueButton;
    public Button mainMenuButton;
    public Slider musicSlider;
    private float musicVolumeValue;
    public Toggle muteButton;

    private bool muteValue;
    public VisualElement pauseMenu;
    public Button settingsBackButton;
    public Button settingsButton;
    public VisualElement settingsMenu;
    public Slider sfxSlider;
    private float sfxVolumeValue;

    // Start is called before the first frame update
    private void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

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
        PauseController.OnPauseDisplay += DisplayPause;
    }

    private void OnDisable()
    {
        PauseController.OnPauseDisplay -= DisplayPause;
    }


    private void DisplayPause(bool isPaused)
    {
        if (isPaused)
            pauseMenu.style.display = DisplayStyle.None;
        else
            pauseMenu.style.display = DisplayStyle.Flex;
    }

    private void ContinueButtonPressed()
    {
        // print("continue");
        Time.timeScale = 1;
        InputManager.ToggleActionMap(InputManager.inputActions.Player);
        DisplayPause(true);
        AudioManager.Instance.PlaySFX(buttonPressSoundEffect);
        // if (OnResume != null)
        //     OnResume();
    }


    private void SettingsButtonPressed()
    {
        AudioManager.Instance.PlaySFX(buttonPressSoundEffect);
        settingsMenu.style.display = DisplayStyle.Flex;
        pauseMenu.style.display = DisplayStyle.None;
    }

    private void MainMenuButtonPressed()
    {
        SceneManager.LoadScene("MainMenuTest");
    }

    private void BackButtonPressed()
    {
        AudioManager.Instance.PlaySFX(buttonPressSoundEffect);
        settingsMenu.style.display = DisplayStyle.None;
        pauseMenu.style.display = DisplayStyle.Flex;
    }
}