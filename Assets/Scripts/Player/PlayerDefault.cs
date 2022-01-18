using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDefault : MonoBehaviour, IPlayer
{
    private PlayerInputActions playerInputActions;
    private InputAction movement;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        movement = playerInputActions.Player.Movement;
        movement.Enable();

        playerInputActions.Player.Jump.performed += Jump;
        playerInputActions.Player.Jump.Enable();
    }


    private void OnDisable()
    {
        movement.Disable();
        playerInputActions.Player.Jump.Disable();
    }

    private void FixedUpdate()
    {
        Move(movement.ReadValue<Vector2>());
    }

    public void Move(Vector2 direction)
    {
        Vector3 movement = direction.x * transform.right + direction.y * transform.forward;
        transform.position += Time.deltaTime * movement.normalized;
    }

    public void Jump(InputAction.CallbackContext obj)
    {
        throw new NotImplementedException();
    }
}
