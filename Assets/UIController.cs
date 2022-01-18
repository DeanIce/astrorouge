using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
public class UIController : MonoBehaviour
{
    public Button newGameButton;
    public Button settingsButton;
    public Button settingsBackButton;
    public Button aboutBackButton;
    public Button quitButton;
    public Button aboutButton;
    public VisualElement settingsMenu;
    public VisualElement mainMenu;
    public VisualElement aboutMenu;

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

        //About buttons
        aboutBackButton = aboutMenu.Q<Button>("back-button");

        newGameButton.clicked += NewGameButtonPressed;
        settingsButton.clicked += SettingsButtonPressed;
        aboutButton.clicked += AboutButtonPressed;
        settingsBackButton.clicked += BackButtonPressed;
        aboutBackButton.clicked += BackButtonPressed;
    }

    void NewGameButtonPressed()
    {
        SceneManager.LoadScene("SampleScene");
    }

    void SettingsButtonPressed()
    {
        settingsMenu.style.display = DisplayStyle.Flex;
        mainMenu.style.display = DisplayStyle.None;
        aboutMenu.style.display = DisplayStyle.None;
    }
    void AboutButtonPressed()
    {
        settingsMenu.style.display = DisplayStyle.None;
        mainMenu.style.display = DisplayStyle.None;
        aboutMenu.style.display = DisplayStyle.Flex;
    }
    void QuitButtonPressed()
    {
        Application.Quit();
    }
    void BackButtonPressed()
    {
        settingsMenu.style.display = DisplayStyle.None;
        mainMenu.style.display = DisplayStyle.Flex;
        aboutMenu.style.display = DisplayStyle.None;
    }
}
