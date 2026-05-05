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

    private void Awake()
    {
        _targetPosition = _defaultOpen ? _openPosition : _closedPosition;
        _doorTransform.localPosition = _targetPosition;
    }

    public void Unlock()
    {
        foreach (var key in _keys)
        {
            if (!key.IsPressed) return;
        }

        _targetPosition = _openPosition;
        _doorTransform.DOLocalMove(_targetPosition, _speed);
    }

    public void Lock()
    {
        _targetPosition = _closedPosition;
        _doorTransform.DOLocalMove(_targetPosition, _speed);
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
