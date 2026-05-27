using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "UIRotateAnimation", menuName = "Animations/UI/RectTransform/Rotate")]
public class UIRotateAnimation : CustomAnimation
{
    [SerializeField] private Vector3 m_TargetRotation;
    public override void Animate(GameObject target)
    {
        if (target.TryGetComponent<RectTransform>(out var transform))
        {
            transform.DOComplete();
            transform.DOLocalRotate(m_TargetRotation, _time).SetEase(_ease);
        }
        else Debug.LogError("The provided GameObject does not have a RectTransform component");
    }
}