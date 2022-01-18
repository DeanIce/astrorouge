using UnityEngine;
using UnityEngine.InputSystem;

public interface IPlayer
{
    public void Move(Vector2 direction);
    public void Jump(InputAction.CallbackContext obj);
}
