using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ImageFillAnimation", menuName = "Animations/UI/Image/Fill")]
public class ImageFillAnimation : CustomAnimation
{
    [SerializeField] private float m_TargetValue = 1;

    public override void Animate(GameObject target)
    {
        if (target.TryGetComponent<Image>(out var img))
        {
            img.DOComplete();
            img.DOFillAmount(m_TargetValue, _time).SetEase(_ease);
        }
        else Debug.LogError("The provided GameObject does not have an Image component");
    }
}