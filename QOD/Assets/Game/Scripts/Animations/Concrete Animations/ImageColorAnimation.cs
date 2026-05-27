using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ImageColorAnimation", menuName = "Animations/UI/Image/Color")]
public class ImageColorAnimation : CustomAnimation
{
    [SerializeField] private Color m_TargetColor = Color.white;

    public override void Animate(GameObject target)
    {
        if (target.TryGetComponent<Image>(out var img))
        {
            img.DOComplete();
            img.DOColor(m_TargetColor, _time).SetEase(_ease);
        }
        else Debug.LogError("The provided GameObject does not have an Image component");
    }
}