using UnityEngine;
using UnityEngine.InputSystem;

public interface IPlayer
{
    public Vector3 Look(Vector2 direction);
    public Vector3 Walk(Vector2 direction);
    public void Jump(InputAction.CallbackContext obj);
    public void SprintToggle(InputAction.CallbackContext obj);
    public void TakeDmg(float dmg);
}
