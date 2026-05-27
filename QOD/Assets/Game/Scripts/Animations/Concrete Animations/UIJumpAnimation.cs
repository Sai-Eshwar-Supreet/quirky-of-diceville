using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "UIJumpAnimation", menuName = "Animations/UI/RectTransform/Jump")]
public class UIJumpAnimation : CustomAnimation
{
    [SerializeField] private Vector2 m_StartAnchoredPos;
    [SerializeField] private Vector2 m_TargetanchoredPos;
    [SerializeField, Min(0.1f)] private float m_JumpPower = 1;
    [SerializeField, Min(1)] private int m_NumberOfJumps = 1;

    public override void Animate(GameObject target)
    {
        if (target.TryGetComponent<RectTransform>(out var transform))
        {
            transform.DOComplete();
            transform.anchoredPosition = m_StartAnchoredPos;
            transform.DOJumpAnchorPos(m_TargetanchoredPos, m_JumpPower, m_NumberOfJumps, _time).SetEase(_ease);
        }
        else Debug.LogError("The provided GameObject does not have a RectTransform component");
    }
}