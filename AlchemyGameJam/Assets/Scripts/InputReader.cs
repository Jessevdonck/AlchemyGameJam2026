using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class InputReader : MonoBehaviour
{
    private InputSystem_Actions controls;

    public Vector2 MoveInput { get; private set; }
    public Vector2 MousePosition { get; private set; }

    public event Action OnShoot;
    public event Action OnDash;

    private void Awake()
    {
        controls = new InputSystem_Actions();

        controls.Player.Move.performed += ctx => MoveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => MoveInput = Vector2.zero;

        controls.Player.Attack.performed += ctx => OnShoot?.Invoke();
        controls.Player.Dash.performed += ctx => OnDash?.Invoke();
        
    }

    private void Update()
    {
        MousePosition = Mouse.current.position.ReadValue();
    }

    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();
}