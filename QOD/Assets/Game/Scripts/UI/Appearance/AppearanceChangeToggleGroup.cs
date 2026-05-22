using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ToggleGroup))]
[RequireComponent(typeof(RectTransform))]
public class AppearanceChangeToggleGroup : MonoBehaviour
{
    [SerializeField] private ToggleGroup _toggleGroup;
    [SerializeField] private RectTransform _rectTransform;
 
    [Header("Animation")]
    [SerializeField] private bool _startOut = true;
    [SerializeField] private Vector2 _inAnchorPos;
    [SerializeField] private float _inDuration = 0.25f;
    [SerializeField] private Ease _inEase = Ease.InOutSine;
    [SerializeField] private Vector2 _outAnchorPos;
    [SerializeField] private float _outDuration = 0.25f;
    [SerializeField] private Ease _outEase = Ease.InOutSine;

    private Tween _moveTween;


    private readonly Dictionary<int, AppearanceChangeToggle> _toggles = new ();

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();

        Move(_startOut);
    }

    public void Move(bool @out)
    {
        _moveTween?.Kill();

        _moveTween = @out ?
            _rectTransform.DOAnchorPos(_outAnchorPos, _outDuration).SetEase(_outEase)
            : _rectTransform.DOAnchorPos(_inAnchorPos, _inDuration).SetEase(_inEase);
    }

    public void Add(int id, AppearanceChangeToggle appearanceToggle)
    {
        appearanceToggle.transform.SetParent(_toggleGroup.transform, false);
        appearanceToggle.Toggle.group = _toggleGroup;
        _toggles.Add(id, appearanceToggle);
    }

    public void SetInteractable(int id, bool interactable)
    {
        if (_toggles.TryGetValue(id, out var toggle))
        {
            toggle.Toggle.interactable = interactable;
        }
    }

    public void Select(int id)
    {
        if (_toggles.TryGetValue(id, out var toggle))
        {
            toggle.Toggle.SetIsOnWithoutNotify(true);
        }
    }
}
