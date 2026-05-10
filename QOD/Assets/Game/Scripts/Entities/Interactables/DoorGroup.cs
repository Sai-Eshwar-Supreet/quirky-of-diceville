using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class DoorGroup : MonoBehaviour
{
    [SerializeField] private Transform _doorTransform;
    [SerializeField] private Vector3 _openPosition;
    [SerializeField] private Vector3 _closedPosition;
    [SerializeField] private float _speed;

    private readonly HashSet<Door> _doorList = new();

    private Vector3 _targetPosition;
    private Tween _moveTween;

    private void Awake()
    {
        _targetPosition = _closedPosition;
        _doorTransform.localPosition = _targetPosition;
    }
    private void OnDestroy()
    {
        if (_moveTween.IsActive()) _moveTween?.Kill();
    }

    public void RegisterDoor(Door door)
    {
        _doorList.Add(door);
        door.OnLockStateUpdated += UpdateState;
    }

    public void UnregisterDoor(Door door)
    {
        door.OnLockStateUpdated -= UpdateState;
        _doorList.Remove(door);
    }

    public bool IsUnlocked()
    {
        foreach (var door in _doorList)
        {
            if (!door.IsUnlocked) return false;
        }

        return true;
    }

    private void UpdateState()
    {
        _targetPosition = IsUnlocked() ? _openPosition : _closedPosition;

        MoveDoor();
    }

    private void MoveDoor()
    {
        if (_moveTween.IsActive()) _moveTween?.Kill();
        _moveTween = _doorTransform.DOLocalMove(_targetPosition, _speed);
        _moveTween.onComplete += () => _moveTween = null;
    }

    private void OnDrawGizmosSelected()
    {
        if (_doorTransform == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position + _openPosition, 0.1f);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position + _closedPosition, 0.1f);
    }
}
