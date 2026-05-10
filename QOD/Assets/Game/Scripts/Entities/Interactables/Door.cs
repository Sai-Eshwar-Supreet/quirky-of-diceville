using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class Door : MonoBehaviour
{
    [SerializeField] private List<Key> _keys;
    [SerializeField] private bool _defaultOpen = false;
    [SerializeField] private Transform _doorTransform;
    [SerializeField] private Vector3 _openPosition;
    [SerializeField] private Vector3 _closedPosition;
    [SerializeField] private float _speed;

    private Vector3 _targetPosition;
    private Tween _moveTween;

    private void Awake()
    {
        _targetPosition = _defaultOpen ? _openPosition : _closedPosition;
        _doorTransform.localPosition = _targetPosition;
    }
    private void OnDestroy()
    {
        if(_moveTween.IsActive()) _moveTween?.Kill();
    }

    public void Unlock()
    {
        foreach (var key in _keys)
        {
            if (!key.IsPressed) return;
        }
        _targetPosition = _openPosition;

        MoveDoor();
    }

    public void Lock()
    {
        _targetPosition = _closedPosition;

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
