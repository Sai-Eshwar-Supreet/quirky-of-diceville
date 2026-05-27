
using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "ScaleAnimation", menuName = "Animations/Transform/Scale")]
public class ScaleAnimation : CustomAnimation
{
    [SerializeField] private Vector3 m_TargetValue = Vector3.one;

    public override void Animate(GameObject target)
    {
        if (target.TryGetComponent<Transform>(out var transform))
        {
            transform.DOComplete();
            transform.DOScale(m_TargetValue, _time).SetEase(_ease);
        }
        else Debug.LogError("The provided GameObject does not have a Transform component");
    }
}