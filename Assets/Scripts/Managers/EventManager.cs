using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    /// <summary>
    ///     This script requires a custom execution order.
    ///     Navigate to "Project Settings > Script Execution Order"
    ///     then add this script at -1 before the default time.
    /// </summary>
    [ExecuteInEditMode]
    public class EventManager : ManagerSingleton<EventManager>
    {
        [HideInInspector] public int requestedScene;

        public string scenePlay;

        public readonly RunStats runStats = new();

        // public static EventManager instance { get; private set; }

        private Mode mode = Mode.Play;


        // Game State events
        public event Action pauseGame, playGame, menu, win, recap, exit;


        // Settings event
        public event Action<UserSettings> settingsUpdated;

        // Player UI events (Todo: Dennis)
        public event Action playerStatsChanged;

        public void Pause()
        {
            LOG("request pause");
            mode = Mode.Pause;
            Time.timeScale = 0f;
            InputManager.ToggleActionMap(InputManager.inputActions.PauseMenu);
            pauseGame?.Invoke();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        public void Play()
        {
            LOG("request play");
            if (mode != Mode.Pause) SceneManager.LoadScene(scenePlay);

            mode = Mode.Play;
            Time.timeScale = 1;
            InputManager.ToggleActionMap(InputManager.inputActions.Player);
            playGame?.Invoke();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        public void Menu()
        {
            LOG("request menu");
            mode = Mode.Menu;
            InputManager.ToggleActionMap(InputManager.inputActions.PauseMenu);
            menu?.Invoke();
            SceneManager.LoadScene("MainMenuTest");
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        public void Win()
        {
            LOG("request win");
            mode = Mode.Win;
            InputManager.ToggleActionMap(InputManager.inputActions.PauseMenu);
            win?.Invoke();
            SceneManager.LoadScene("Win");
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        public void Recap()
        {
            LOG("request recap");
            mode = Mode.Recap;
            InputManager.ToggleActionMap(InputManager.inputActions.PauseMenu);
            recap?.Invoke();
            SceneManager.LoadScene("Recap");
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }


        public void Exit()
        {
            exit?.Invoke();
            Application.Quit();
        }

        public void UpdateSettings(UserSettings settings, string name)
        {
            PersistentUpgrades.Save(settings, name);
            settingsUpdated?.Invoke(settings);
        }

        private enum Mode
        {
            Play,
            Pause,
            Menu,
            Win,
            Recap,
            Loading
        }
    }
}