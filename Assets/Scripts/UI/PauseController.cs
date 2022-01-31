using UnityEngine;
using UnityEngine.InputSystem;

public class PauseController : MonoBehaviour
{
    public delegate void PauseDisplay(bool paused);

    private void OnEnable()
    {
        var playerInputMap = InputManager.inputActions.Player;
        var pauseInputMap = InputManager.inputActions.PauseMenu;

        pauseInputMap.Back.performed += PlayGame;
        playerInputMap.PauseGame.performed += PauseGame;
    }

    private void OnDisable()
    {
        var playerInputMap = InputManager.inputActions.Player;
        var pauseInputMap = InputManager.inputActions.PauseMenu;

        pauseInputMap.Back.performed -= PlayGame;

        playerInputMap.PauseGame.performed -= PauseGame;
    }

    public static event PauseDisplay OnPauseDisplay;


    public void PauseGame(InputAction.CallbackContext obj)
    {
        Time.timeScale = 0f;
        // print("Pause received");
        OnPauseDisplay?.Invoke(false);
        InputManager.ToggleActionMap(InputManager.inputActions.PauseMenu);
    }

    public void PlayGame(InputAction.CallbackContext obj)
    {
        Time.timeScale = 1;
        // print("Play received");
        OnPauseDisplay?.Invoke(true);
        InputManager.ToggleActionMap(InputManager.inputActions.Player);
    }
}