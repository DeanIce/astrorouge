using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static PlayerInputActions inputActions = new();

    private void Start()
    {
        // Start with the player input map set
        ToggleActionMap(inputActions.Player);
    }

    public static event Action<InputActionMap> actionMapChange;

    public static void ToggleActionMap(InputActionMap actionMap)
    {
        print($"Map toggled {actionMap.name}");
        if (actionMap.enabled) return;
        inputActions.Disable();
        actionMapChange?.Invoke(actionMap);
        actionMap.Enable();
    }
}