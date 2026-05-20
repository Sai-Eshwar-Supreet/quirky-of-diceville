using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingUI : MonoBehaviour
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private TextMeshProUGUI _loadingText;
    [SerializeField] private Image _loadingProgressBar;


    [Header("Progress Smoothing")]
    [SerializeField] private float _progressSpeed = 1.5f;

    private float _targetProgress;
    private float _currentProgress;

    public bool IsFinished => Mathf.Approximately(_currentProgress, 1f);

    public void SetActive(bool isActive)
    {
        _panel.SetActive(isActive);


        if (isActive)
        {
            ResetProgress();
        }
    }

    public void SetTargetProgress(float progress)
    {
        _targetProgress = Mathf.Clamp01(progress);
    }

    private void Update()
    {
        if (Mathf.Approximately(_currentProgress, _targetProgress)) return;

        _currentProgress = Mathf.MoveTowards(
            _currentProgress,
            _targetProgress,
            _progressSpeed * Time.deltaTime
        );

        UpdateVisuals();
    }

    private void ResetProgress()
    {
        _targetProgress = 0f;
        _currentProgress = 0f;

        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        _loadingProgressBar.fillAmount = _currentProgress;
        _loadingText.SetText($"{Mathf.RoundToInt(_currentProgress * 100)}%");
    }
}
