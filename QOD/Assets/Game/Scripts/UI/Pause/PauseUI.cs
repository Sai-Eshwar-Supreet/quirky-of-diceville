using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Button _continueButton;

    [Header("Animation")]
    [SerializeField] private float _openDuration = 0.25f;
    [SerializeField] private float _closeDuration = 0.25f;


    [Header("Sounds")]
    [SerializeField] private SoundConfig _openSound;
    [SerializeField] private SoundConfig _closeSound;

    public event System.Action OnOpen;
    public event System.Action OnClose;

    private void Awake()
    {
        _continueButton.onClick.AddListener(Close);

        Close();
    }

    private void OnDestroy()
    {
        if(_continueButton != null) _continueButton.onClick.RemoveListener(Close);
    }
    public bool IsOpen { get; private set; }

    private Tween _fadeTween;

    public void Open()
    {
        IsOpen = true;
        SoundManager.Play(_openSound);
        _canvasGroup.blocksRaycasts = true;
        OnOpen?.Invoke();

        _fadeTween?.Kill();
        _fadeTween = _canvasGroup.DOFade(1, _openDuration).SetEase(Ease.InOutSine);
    }

    public void Close()
    {
        IsOpen = false;
        SoundManager.Play(_closeSound);
        _fadeTween?.Kill();

        _fadeTween = _canvasGroup.DOFade(0, _closeDuration).OnComplete(() =>
        {
            _canvasGroup.blocksRaycasts = false;
            OnClose?.Invoke();
        }).SetEase(Ease.InOutSine);
    }
}
