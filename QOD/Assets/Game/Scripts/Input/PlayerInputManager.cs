using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager
{
    private PlayerInput.PlayerActions _playerActions;

    public event Action<Vector2> OnMove;
    public event Action<bool> OnAppearanceMenuInput;
    public event Action<int> OnPlayerSwitch;

    public void Init()
    {
        _playerActions = new PlayerInput().Player;
    }

    public void SetCursorState(bool isLocked)
    {
        if (isLocked) CursorHandler.Lock();
        else CursorHandler.Unlock();
    }

    public void Enable()
    {
        _playerActions.Enable();

        _playerActions.Move.performed += MoveEventHandler;
        _playerActions.Move.canceled += MoveEventHandler;

        _playerActions.ChangeAppearance.performed += OnAppreanceMenuEventHandler;
        _playerActions.ChangeAppearance.canceled += OnAppreanceMenuEventHandler;

        _playerActions.Switch.performed += SwitchEventHandler;
    }

    public void Disable()
    {
        _playerActions.Move.performed -= MoveEventHandler;
        _playerActions.Move.canceled -= MoveEventHandler;

        _playerActions.ChangeAppearance.performed -= OnAppreanceMenuEventHandler;
        _playerActions.ChangeAppearance.canceled -= OnAppreanceMenuEventHandler;

        _playerActions.Switch.performed -= SwitchEventHandler;

        _playerActions.Disable();
    }

    private void MoveEventHandler(InputAction.CallbackContext context)
    {
        var move = context.ReadValue<Vector2>();
        if (move.x != 0 && move.y != 0) move.x = 0;

        OnMove?.Invoke(move);
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
