using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenu : MonoBehaviour
{
    public AudioClip mainMenuMusic;
    public AudioClip buttonPressSoundEffect;
    public string mainScene;
    public Button aboutBackButton;
    public Button aboutButton;
    public VisualElement aboutMenu;
    public VisualElement mainMenu;
    public Slider musicSlider;
    private float musicVolumeValue;
    public Toggle muteButton;

    private bool muteValue;

    public Button newGameButton;
    public Button quitButton;
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

    private void NewGameButtonPressed()
    {
        AudioManager.Instance.PlaySFX(buttonPressSoundEffect);
        SceneManager.LoadScene(mainScene);
        EventManager.instance.Play();
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
    }

    private void QuitButtonPressed()
    {
        AudioManager.Instance.PlaySFX(buttonPressSoundEffect);
        Application.Quit();
    }

    private void BackButtonPressed()
    {
        AudioManager.Instance.PlaySFX(buttonPressSoundEffect);
        settingsMenu.style.display = DisplayStyle.None;
        mainMenu.style.display = DisplayStyle.Flex;
        aboutMenu.style.display = DisplayStyle.None;
    }
}