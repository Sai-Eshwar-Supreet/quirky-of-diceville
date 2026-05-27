using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "ShakeAnimation", menuName = "Animations/Transform/Shake")]
public class ShakeAnimation : CustomAnimation
{
    [System.Serializable]
    public class ShakeSettings
    {
        public float Strength = 1;
        public int Vibrato = 10;
        public float Randomness = 90;
        public bool FadeOut = true;
        public ShakeRandomnessMode Mode = ShakeRandomnessMode.Full;
    }

    [SerializeField] private bool m_ShakePosition, m_ShakeRotation, m_ShakeScale;
    [Header("Position")]
    [SerializeField] private ShakeSettings m_PositionSettings;
    [SerializeField] private bool m_PositionSnapping = false;

    [Header("Rotation")]
    [SerializeField] private ShakeSettings m_RotationSettings;

    [Header("Scale")]
    [SerializeField] private ShakeSettings m_ScaleSettings;

    public override void Animate(GameObject target)
    {
        if (target.TryGetComponent<Transform>(out var transform))
        {
            transform.DOComplete();
            if (m_ShakePosition) transform.DOShakePosition(
                _time,
                m_PositionSettings.Strength,
                m_PositionSettings.Vibrato,
                m_PositionSettings.Randomness,
                m_PositionSnapping,
                m_PositionSettings.FadeOut,
                m_PositionSettings.Mode
                ).SetEase(_ease);

            if (m_ShakeRotation) transform.DOShakeRotation(
                _time,
                m_RotationSettings.Strength,
                m_RotationSettings.Vibrato,
                m_RotationSettings.Randomness,
                m_RotationSettings.FadeOut,
                m_RotationSettings.Mode
                ).SetEase(_ease);

            if (m_ShakeScale) transform.DOShakeScale(
                _time,
                m_ScaleSettings.Strength,
                m_ScaleSettings.Vibrato,
                m_ScaleSettings.Randomness,
                m_ScaleSettings.FadeOut,
                m_ScaleSettings.Mode
                ).SetEase(_ease);

        }
        else Debug.LogError("The provided GameObject does not have a Transform component");
    }
}