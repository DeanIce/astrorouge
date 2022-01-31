using UnityEngine;
using UnityEngine.InputSystem;

public class PauseController : MonoBehaviour
{
    public delegate void PauseDisplay(bool paused);

    public static bool isPaused;

    private void Start()
    {
        // action.Pause.PauseGame.performed += _ => Pause();
    }


    private void OnEnable()
    {
        var playerInputMap = InputManager.inputActions.Player;
        var pauseInputMap = InputManager.inputActions.PauseMenu;

        pauseInputMap.Back.performed += PlayGame;
        PauseMenu.OnResume += Pause;
        // action.Enable();
    }

    private void OnDisable()
    {
        var playerInputMap = InputManager.inputActions.Player;
        var pauseInputMap = InputManager.inputActions.PauseMenu;

        pauseInputMap.Back.performed -= PlayGame;

        PauseMenu.OnResume -= Pause;
        // action.Disable();
    }

    public static event PauseDisplay OnPauseDisplay;


    private void PlayGame(InputAction.CallbackContext obj)
    {
        print("Play received");
        InputManager.ToggleActionMap(InputManager.inputActions.Player);
    }

    public void Pause()
    {
        if (isPaused)
        {
            Time.timeScale = 0f;
            isPaused = !isPaused;
            OnPauseDisplay?.Invoke(isPaused);
        }
        else
        {
            Time.timeScale = 1;
            isPaused = !isPaused;
            OnPauseDisplay?.Invoke(isPaused);
        }
    }
}