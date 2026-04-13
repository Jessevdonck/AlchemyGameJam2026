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

    public event Action OnNextPotion;

    public event Action OnUsePotion;

    private void Awake()
    {
        controls = new InputSystem_Actions();
    }

    private void Update()
    {
        MousePosition = Mouse.current.position.ReadValue();
        
    }

    private void OnEnable()
    {
        controls.Enable();
        
        controls.Player.Move.performed += ctx => MoveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => MoveInput = Vector2.zero;

        controls.Player.Attack.performed += ctx => OnShoot?.Invoke();
        controls.Player.Dash.performed += ctx => OnDash?.Invoke();

        controls.Player.UsePotion.performed += ctx => OnUsePotion?.Invoke();

        controls.Player.CyclePotion.performed += ctx => OnNextPotion?.Invoke();
    }

    private void OnDisable()
    {
        
        controls.Disable(); 
        controls.Player.Move.performed -= ctx => MoveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled -= ctx => MoveInput = Vector2.zero;

        controls.Player.Attack.performed -= ctx => OnShoot?.Invoke();
        controls.Player.Dash.performed -= ctx => OnDash?.Invoke();

        controls.Player.UsePotion.performed -= ctx => OnUsePotion?.Invoke();

        controls.Player.CyclePotion.performed -= ctx => OnNextPotion.Invoke();
    }
}