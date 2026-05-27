using DG.Tweening;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "TextColorAnimation", menuName = "Animations/UI/Text/Color")]
public class TextColorAnimation : CustomAnimation
{
    [SerializeField] private Color _targetValue = Color.white;

    public override void Animate(GameObject target)
    {
        if (target.TryGetComponent<TextMeshProUGUI>(out var text))
        {
            text.DOComplete();
            text.DOColor(_targetValue, _time).SetEase(_ease);
        }
        else Debug.LogError("The provided GameObject does not have an TextMeshProUGUI component");
    }
}