using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    /// <summary>
    ///     This script requires a custom execution order.
    ///     Navigate to "Project Settings > Script Execution Order"
    ///     then add this script at -1 before the default time.
    /// </summary>
    public class EventManager : ManagerSingleton<EventManager>
    {
        [HideInInspector] public int requestedScene;

        public string scenePlay;

        public readonly Dictionary<string, (AbstractItem, int)> inventory = new();

        private UserSettings _user;


        public Action<int> loadLevel;


        private Mode mode = Mode.Play;

        public RunStats runStats = new();

        public UserSettings user
        {
            get
            {
                if (_user == null) _user = PersistentUpgrades.Load<UserSettings>("userSettings");
                return _user;
            }
        }

        // private void Update()
        // {
        //     print(Time.frameCount);
        // }


        // Game State events
        public event Action pauseGame,
            playGame,
            planetCleared,
            win,
            menu,
            recap,
            exit,
            loadBoss,
            playerStatsUpdated,
            enemyDamaged;

        // Settings event
        public event Action<UserSettings> settingsUpdated;

        public event Action<AbstractItem> itemAcquired;


        public event Action<float> crosshairSpread, playerDamaged, specialUsed, meleeUsed, secondaryUsed;

        public void PlanetCleared()
        {
            planetCleared?.Invoke();
        }

        public void CrosshairSpread(float a)
        {
            crosshairSpread?.Invoke(a);
        }

        public void SpecialUsed(float a)
        {
            specialUsed?.Invoke(a);
        }

        public void MeleeUsed(float a)
        {
            meleeUsed?.Invoke(a);
        }

        public void SecondaryUsed(float a)
        {
            secondaryUsed?.Invoke(a);
        }

        public void ItemAcquired(AbstractItem item)
        {
            itemAcquired?.Invoke(item);
        }

        public void PlayerStatsUpdated()
        {
            playerStatsUpdated?.Invoke();
        }

        public void EnemyDamaged()
        {
            enemyDamaged?.Invoke();
        }

        public void PlayerDamaged(float percent)
        {
            playerDamaged?.Invoke(percent);
        }

        public void LoadLevel(int i)
        {
            SceneManager.LoadScene("LevelScene");
            LevelSelect.Instance.requestedLevel = i;
            PlayerStatsUpdated();
        }

        public void LoadBoss(string bossSceneName)
        {
            SceneManager.LoadScene(bossSceneName);
            loadBoss?.Invoke();
            PlayerStatsUpdated();
        }

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
            // Below happens when we request play from the main menu,
            // signifying the start of a new run with zeroed stats.
            if (mode != Mode.Pause)
            {
                SceneManager.LoadScene(scenePlay);
                resetInternalState();
            }

            mode = Mode.Play;
            Time.timeScale = 1;
            InputManager.ToggleActionMap(InputManager.inputActions.Player);
            playGame?.Invoke();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        public void UpgradeMenu()
        {
            LOG("Request Upgrade Menu");

            SceneManager.LoadScene("MetaProgression");
        }

        private void resetInternalState()
        {
            LevelSelect.Instance.requestedLevel = 0;
            runStats = new RunStats();
            PlayerStats.Instance.SetDefaultValues();
            PersistentUpgradeManager.Instance.ApplyPersistentStats();
            inventory.Clear();
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
            PersistentData.Save(settings, name);
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