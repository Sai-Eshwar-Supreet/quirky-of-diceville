using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private DoorGroup _doorGroup;
    [SerializeField] private DoorVisual _doorVisual;

    public System.Action OnLockStateUpdated;

    public bool IsUnlocked { get; private set; }
    private void OnEnable()
    {
        _doorGroup.RegisterDoor(this);
        _doorVisual.SetLockState(false);
    }

    private void OnDisable()
    {
        _doorGroup.UnregisterDoor(this);
    }

    public void SetLockState(bool unlocked)
    {
        if (IsUnlocked == unlocked) return;

        IsUnlocked = unlocked;
        _doorVisual.SetLockState(unlocked);
        OnLockStateUpdated?.Invoke();
    }
}
