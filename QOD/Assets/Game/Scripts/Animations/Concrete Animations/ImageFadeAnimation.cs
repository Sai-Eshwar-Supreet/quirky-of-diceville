using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ImageFadeAnimation", menuName = "Animations/UI/Image/Fade")]
public class ImageFadeAnimation : CustomAnimation
{
    [SerializeField] private float m_TargetOpacity = 1;

    public override void Animate(GameObject target)
    {
        if (target.TryGetComponent<Image>(out var img))
        {
            img.DOComplete();
            img.DOFade(m_TargetOpacity, _time).SetEase(_ease);
        }
        else Debug.LogError("The provided GameObject does not have an Image component");
    }
}