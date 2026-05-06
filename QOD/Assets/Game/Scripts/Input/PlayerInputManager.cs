using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager
{
    private PlayerInput.PlayerActions _playerActions;

    public event Action<Vector2> OnMove;
    public event Action<bool> OnHover;
    public event Action<bool> OnAppearanceMenuInput;
    public event Action<int> OnPlayerSwitch;


    public void Init()
    {
        _playerActions = new PlayerInput().Player;
    }

    public void Enable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _playerActions.Enable();

        _playerActions.Move.performed += MoveEventHandler;
        _playerActions.Move.canceled += MoveEventHandler;

        _playerActions.Hover.performed += HoverEventHandler;
        _playerActions.Hover.canceled += HoverEventHandler;

        _playerActions.ChangeAppearance.performed += OnAppreanceMenuEventHandler;
        _playerActions.ChangeAppearance.canceled += OnAppreanceMenuEventHandler;

        _playerActions.Switch.performed += SwitchEventHandler;
    }

    public void Disable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        _playerActions.Move.performed -= MoveEventHandler;
        _playerActions.Move.canceled -= MoveEventHandler;

        _playerActions.Hover.performed -= HoverEventHandler;
        _playerActions.Hover.canceled -= HoverEventHandler;

        _playerActions.ChangeAppearance.performed -= OnAppreanceMenuEventHandler;
        _playerActions.ChangeAppearance.canceled -= OnAppreanceMenuEventHandler;

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
    private void OnAppreanceMenuEventHandler(InputAction.CallbackContext context)
    {
        OnAppearanceMenuInput?.Invoke(context.ReadValueAsButton());
    }
    private void SwitchEventHandler(InputAction.CallbackContext context)
    {
        OnPlayerSwitch?.Invoke((int)context.ReadValue<float>());
    }
}
