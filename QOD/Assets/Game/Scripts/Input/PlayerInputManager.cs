using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager
{
    private PlayerInput.PlayerActions _playerActions;

    public event Action<Vector2> OnMove;
    public event Action<bool> OnHover;
    public event Action OnColorChange;
    public event Action OnValueChange;
    public event Action OnPlayerSwitch;


    public void Init()
    {
        _playerActions = new PlayerInput().Player;
    }

    public void Enable()
    {
        _playerActions.Enable();

        _playerActions.Move.performed += MoveEventHandler;
        _playerActions.Move.canceled += MoveEventHandler;

        _playerActions.Hover.performed += HoverEventHandler;
        _playerActions.Hover.canceled += HoverEventHandler;

        _playerActions.ChangeColor.performed += ColorChangeEventHandler;

        _playerActions.ChangeValue.performed += ValueChangeEventHandler;

        _playerActions.Switch.performed += SwitchEventHandler;
    }

    public void Disable()
    {
        _playerActions.Move.performed -= MoveEventHandler;
        _playerActions.Move.canceled -= MoveEventHandler;

        _playerActions.Hover.performed -= HoverEventHandler;
        _playerActions.Hover.canceled -= HoverEventHandler;

        _playerActions.ChangeColor.performed -= ColorChangeEventHandler;

        _playerActions.ChangeValue.performed -= ValueChangeEventHandler;

        _playerActions.Switch.performed -= SwitchEventHandler;

        _playerActions.Disable();
    }

    private void MoveEventHandler(InputAction.CallbackContext context)
    {
        OnMove?.Invoke(context.ReadValue<Vector2>());
    }
    private void HoverEventHandler(InputAction.CallbackContext context)
    {
        OnHover?.Invoke(context.ReadValueAsButton());
    }
    private void ColorChangeEventHandler(InputAction.CallbackContext context)
    {
        OnColorChange?.Invoke();
    }
    private void ValueChangeEventHandler(InputAction.CallbackContext context)
    {
        OnValueChange?.Invoke();
    }
    private void SwitchEventHandler(InputAction.CallbackContext context)
    {
        OnPlayerSwitch?.Invoke();
    }
}
