using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class DoorGroup : MonoBehaviour
{
    [SerializeField] private Transform _doorTransform;
    [SerializeField] private Vector3 _openPosition;
    [SerializeField] private Vector3 _closedPosition;
    [SerializeField] private float _openSpeed;
    [SerializeField] private float _closeSpeed;
    [SerializeField] private Ease _openEase = Ease.InOutSine;
    [SerializeField] private Ease _closeEase = Ease.InOutSine;

    private readonly HashSet<Door> _doorList = new();

    private Tween _moveTween;

    private void Awake()
    {
        _doorTransform.localPosition = _closedPosition;
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
        var isUnlocked = IsUnlocked();
        var targetPosition =  isUnlocked ? _openPosition : _closedPosition;
        var speed = isUnlocked ? _openSpeed : _closeSpeed;
        var ease = isUnlocked ? _openEase : _closeEase;
        MoveDoor(targetPosition, speed, ease);
    }

    private void MoveDoor(Vector3 position, float speed, Ease ease)
    {
        if (_moveTween.IsActive()) _moveTween?.Kill();
        _moveTween = _doorTransform.DOLocalMove(position, speed).SetEase(ease);
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
