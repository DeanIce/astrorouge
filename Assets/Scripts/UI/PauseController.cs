using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseController : MonoBehaviour
{
    public delegate void PauseDisplay(bool paused);
    public static event PauseDisplay OnPauseDisplay;
    public static bool isPaused;

    private PauseAction action;

    private void Awake()
    {
        action = new PauseAction();
    }

    private void OnEnable()
    {
        PauseMenu.OnResume += Pause;
        action.Enable();
    }
    private void OnDisable()
    {
        PauseMenu.OnResume -= Pause;
        action.Disable();
    }

    private void Start()
    {
        action.Pause.PauseGame.performed += _ => Pause();
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
