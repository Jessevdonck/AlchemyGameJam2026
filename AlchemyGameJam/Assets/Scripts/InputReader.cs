using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class InputReader : MonoBehaviour
{
    private InputSystem_Actions controls;

    public Vector2 MoveInput { get; private set; }
    public Vector2 MousePosition => controls.Player.Mouse.ReadValue<Vector2>();

    public event Action OnShoot;
    public event Action OnDash;
    public event Action OnNextPotion;
    public event Action OnUsePotion;
    public event Action OnInteract; 
    public event Action OnUseAbilityOne;
    public event Action OnUseAbilityTwo;
    public event Action OnUseAbilityThree;
    public event Action OnUseAbilityFour;

    private Action<InputAction.CallbackContext> _onMove;
    private Action<InputAction.CallbackContext> _onMoveCanceled;
    private Action<InputAction.CallbackContext> _onShoot;
    private Action<InputAction.CallbackContext> _onDash;
    private Action<InputAction.CallbackContext> _onUsePotion;
    private Action<InputAction.CallbackContext> _onCyclePotion;
    private Action<InputAction.CallbackContext> _onInteract; 
    private Action<InputAction.CallbackContext> _onAbilityOne;
    private Action<InputAction.CallbackContext> _onAbilityTwo;
    private Action<InputAction.CallbackContext> _onAbilityThree;
    private Action<InputAction.CallbackContext> _onAbilityFour;

    private void Awake()
    {
        controls = new InputSystem_Actions();

        _onMove          = ctx => MoveInput = ctx.ReadValue<Vector2>();
        _onMoveCanceled  = ctx => MoveInput = Vector2.zero;
        _onShoot         = ctx => OnShoot?.Invoke();
        _onDash          = ctx => OnDash?.Invoke();
        _onUsePotion     = ctx => OnUsePotion?.Invoke();
        _onCyclePotion   = ctx => OnNextPotion?.Invoke();
        _onInteract      = ctx => OnInteract?.Invoke(); 
        _onAbilityOne    = ctx => OnUseAbilityOne?.Invoke();
        _onAbilityTwo    = ctx => OnUseAbilityTwo?.Invoke();
        _onAbilityThree  = ctx => OnUseAbilityThree?.Invoke();
        _onAbilityFour   = ctx => OnUseAbilityFour?.Invoke();
    }

    private void OnEnable()
    {
        controls.Enable();

        controls.Player.Move.performed        += _onMove;
        controls.Player.Move.canceled         += _onMoveCanceled;
        controls.Player.Attack.performed      += _onShoot;
        controls.Player.Dash.performed        += _onDash;
        controls.Player.UsePotion.performed   += _onUsePotion;
        controls.Player.CyclePotion.performed += _onCyclePotion;
        controls.Player.Interact.performed += _onInteract;
        controls.Player.AbilityOne.performed  += _onAbilityOne;
        controls.Player.AbilityTwo.performed  += _onAbilityTwo;
        controls.Player.AbilityThree.performed += _onAbilityThree;
        controls.Player.AbilityFour.performed += _onAbilityFour;
    }

    private void OnDisable()
    {
        controls.Player.Move.performed        -= _onMove;
        controls.Player.Move.canceled         -= _onMoveCanceled;
        controls.Player.Attack.performed      -= _onShoot;
        controls.Player.Dash.performed        -= _onDash;
        controls.Player.UsePotion.performed   -= _onUsePotion;
        controls.Player.CyclePotion.performed -= _onCyclePotion;
        controls.Player.Interact.performed    -= _onInteract;
        controls.Player.AbilityOne.performed  -= _onAbilityOne;
        controls.Player.AbilityTwo.performed  -= _onAbilityTwo;
        controls.Player.AbilityThree.performed -= _onAbilityThree;
        controls.Player.AbilityFour.performed -= _onAbilityFour;

        controls.Disable();
    }
}