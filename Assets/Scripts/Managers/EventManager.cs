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
    public class EventManager : MonoBehaviour
    {
        public enum Mode
        {
            Play,
            Pause,
            Menu,
            Win,
            Recap,
            Loading
        }

        public bool logEvents = true;

        public string scenePlay;


        public static EventManager instance { get; private set; }

        public Mode mode { get; private set; }

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }


        // Game State events
        public event Action pauseGame, playGame, menu, win, recap, loading;

        // Player UI events (Todo: Dennis)
        public event Action playerStatsChanged;

        public void Pause()
        {
            log("request pause");
            mode = Mode.Pause;
            Time.timeScale = 0f;
            InputManager.ToggleActionMap(InputManager.inputActions.PauseMenu);
            pauseGame?.Invoke();
        }

        public void Play()
        {
            log("request play");
            if (mode != Mode.Pause) SceneManager.LoadScene(scenePlay);
            mode = Mode.Play;
            Time.timeScale = 1;
            InputManager.ToggleActionMap(InputManager.inputActions.Player);
            playGame?.Invoke();
        }

        public void Menu()
        {
            log("request menu");
            mode = Mode.Menu;
            InputManager.ToggleActionMap(InputManager.inputActions.PauseMenu);
            menu?.Invoke();
            SceneManager.LoadScene("MainMenuTest");
        }

        public void Win()
        {
            log("request win");
            mode = Mode.Win;
            InputManager.ToggleActionMap(InputManager.inputActions.PauseMenu);
            win?.Invoke();
            SceneManager.LoadScene("Win");
        }

        private void log(object o)
        {
            if (logEvents) print(o);
        }
    }
}