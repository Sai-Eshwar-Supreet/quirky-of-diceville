using DG.Tweening;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _cameraPivot;
    [SerializeField] private float _moveDuration = 0.1f;
    [SerializeField] private Ease _moveEase = Ease.Linear;
    [SerializeField] private float _rotateDuration = 0.1f;
    [SerializeField] private Ease _rotateEase = Ease.Linear;

    private Sequence _cameraTween;

    public void MoveImmediateTo(Transform transform)
    {
        _cameraTween?.Kill();

        _cameraPivot.SetParent(transform);
        _cameraPivot.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
    }

    public void MoveTo(Transform transform)
    {
        _cameraTween?.Kill();

        _cameraPivot.SetParent(transform, true);


        _cameraTween = DOTween.Sequence(_cameraPivot);
        _cameraTween.Join(_cameraPivot.DOLocalMove(Vector3.zero, _moveDuration).SetEase(_moveEase));
        _cameraTween.Join(_cameraPivot.DOLocalRotateQuaternion(Quaternion.identity, _rotateDuration).SetEase(_rotateEase));
    }
}
