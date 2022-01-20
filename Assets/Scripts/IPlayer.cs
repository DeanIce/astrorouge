using UnityEngine;
using UnityEngine.InputSystem;

public interface IPlayer
{
    public Vector3 Walk(Vector2 direction);
    public void Jump(InputAction.CallbackContext obj);
}
