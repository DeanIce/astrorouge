using System;
using UnityEngine;
using UnityEngine.InputSystem;

// Adapted from https://onewheelstudio.com/blog/2021/6/27/changing-action-maps-with-unitys-new-input-system
public class InputManager : MonoBehaviour
{
    public static PlayerInputActions inputActions = new();
    public static bool log = false;

    private void Start()
    {
        // Start with the player input map set
        ToggleActionMap(inputActions.Player);
    }

    public static event Action<InputActionMap> actionMapChange;

    public static void ToggleActionMap(InputActionMap actionMap)
    {
        if (log) print($"Input Map toggled {actionMap.name}");
        if (actionMap.enabled) return;
        inputActions.Disable();
        actionMapChange?.Invoke(actionMap);
        actionMap.Enable();
    }
}