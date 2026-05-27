using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "UIMoveAnimation", menuName = "Animations/UI/RectTransform/Move")]
public class UIMoveAnimation : CustomAnimation
{
    [SerializeField] private Vector2 m_AnchoredPos;
    public override void Animate(GameObject target)
    {
        if (target.TryGetComponent<RectTransform>(out var transform))
        {
            transform.DOComplete();
            transform.DOAnchorPos(m_AnchoredPos, _time).SetEase(_ease);
        }
        else Debug.LogError("The provided GameObject does not have a RectTransform component");
    }
}