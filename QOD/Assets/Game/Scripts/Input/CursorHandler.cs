using UnityEngine;
using UnityEngine.InputSystem;

public static class CursorHandler
{
    private static Vector2 _lastSavedPosition;
    public static void Lock()
    {
        _lastSavedPosition = Mouse.current.position.ReadValue();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public static void Unlock()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Mouse.current.WarpCursorPosition(_lastSavedPosition);
    }
}
