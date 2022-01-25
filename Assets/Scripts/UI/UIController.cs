using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using Managers;

public class UIController : MonoBehaviour
{
    public Button newGameButton;
    public Button settingsButton;
    public Button settingsBackButton;
    public Button aboutBackButton;
    public Button quitButton;
    public Button aboutButton;
    public Toggle muteButton;
    public Slider musicSlider;
    public Slider sfxSlider;
    public VisualElement settingsMenu;
    public VisualElement mainMenu;
    public VisualElement aboutMenu;

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
        mainMenu = root.Q<VisualElement>("MainMenu");
        aboutMenu = root.Q<VisualElement>("AboutMenu");

        //Main Menu buttons
        newGameButton = mainMenu.Q<Button>("newgame-button");
        settingsButton = mainMenu.Q<Button>("settings-button");
        aboutButton = mainMenu.Q<Button>("about-button");
        quitButton = mainMenu.Q<Button>("quit-button");

        //Settings buttons
        settingsBackButton = settingsMenu.Q<Button>("back-button");
        muteButton = settingsMenu.Q<Toggle>("mute-button");
        musicSlider = settingsMenu.Q<Slider>("music-volume-slider");
        sfxSlider = settingsMenu.Q<Slider>("sfx-volume-slider");

        //About buttons
        aboutBackButton = aboutMenu.Q<Button>("back-button");

        quitButton.clicked += QuitButtonPressed;
        newGameButton.clicked += NewGameButtonPressed;
        settingsButton.clicked += SettingsButtonPressed;
        aboutButton.clicked += AboutButtonPressed;
        settingsBackButton.clicked += BackButtonPressed;
        aboutBackButton.clicked += BackButtonPressed;

        muteValue = muteButton.value;
        musicVolumeValue = musicSlider.value;
        sfxVolumeValue = sfxSlider.value;

        AudioManager.Instance.PlayMusic(mainMenuMusic);
    }

    void NewGameButtonPressed()
    {
        AudioManager.Instance.PlaySFX(buttonPressSoundEffect);
        SceneManager.LoadScene("SampleScene");
    }

    void SettingsButtonPressed()
    {
        AudioManager.Instance.PlaySFX(buttonPressSoundEffect);
        settingsMenu.style.display = DisplayStyle.Flex;
        mainMenu.style.display = DisplayStyle.None;
        aboutMenu.style.display = DisplayStyle.None;
    }
    void AboutButtonPressed()
    {
        AudioManager.Instance.PlaySFX(buttonPressSoundEffect);
        settingsMenu.style.display = DisplayStyle.None;
        mainMenu.style.display = DisplayStyle.None;
        aboutMenu.style.display = DisplayStyle.Flex;
    }
    void QuitButtonPressed()
    {
        AudioManager.Instance.PlaySFX(buttonPressSoundEffect);
        Application.Quit();
    }
    void BackButtonPressed()
    {
        AudioManager.Instance.PlaySFX(buttonPressSoundEffect);
        settingsMenu.style.display = DisplayStyle.None;
        mainMenu.style.display = DisplayStyle.Flex;
        aboutMenu.style.display = DisplayStyle.None;
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
