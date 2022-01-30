using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using Managers;

public class PauseMenu : MonoBehaviour
{
    public delegate void Resume();
    public static event Resume OnResume;

    public Button continueButton;
    public Button settingsButton;
    public Button settingsBackButton;
    public Button mainMenuButton;
    public Toggle muteButton;
    public Slider musicSlider;
    public Slider sfxSlider;
    public VisualElement settingsMenu;
    public VisualElement pauseMenu;

    private bool muteValue;
    private float musicVolumeValue;
    private float sfxVolumeValue;

    public AudioClip mainMenuMusic;
    public AudioClip buttonPressSoundEffect;

    // Start is called before the first frame update
    void Start()
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
    private void OnEnable()
    {
        PauseController.OnPauseDisplay += DisplayPause;
    }
    private void OnDisable()
    {
        PauseController.OnPauseDisplay -= DisplayPause;
    }

    void DisplayPause(bool isPaused)
    {
        if (isPaused)
        {
            pauseMenu.style.display = DisplayStyle.None;
        }
        else
        {
            pauseMenu.style.display = DisplayStyle.Flex;
        }
    }

    void ContinueButtonPressed()
    {
        AudioManager.Instance.PlaySFX(buttonPressSoundEffect);
        if (OnResume != null)
            OnResume();
    }


    void SettingsButtonPressed()
    {
        AudioManager.Instance.PlaySFX(buttonPressSoundEffect);
        settingsMenu.style.display = DisplayStyle.Flex;
        pauseMenu.style.display = DisplayStyle.None;
    }
    void MainMenuButtonPressed()
    {
        SceneManager.LoadScene("MainMenuTest");
    }
    void BackButtonPressed()
    {
        AudioManager.Instance.PlaySFX(buttonPressSoundEffect);
        settingsMenu.style.display = DisplayStyle.None;
        pauseMenu.style.display = DisplayStyle.Flex;
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
}
