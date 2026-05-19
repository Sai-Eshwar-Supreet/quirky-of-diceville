using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingUI : MonoBehaviour
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private TextMeshProUGUI _loadingText;
    [SerializeField] private Image _loadingProgressBar;

    public void SetActive(bool isActive)
    {
        _panel.SetActive(isActive);
    }

    public void UpdateProgress(float progress)
    {
        _loadingProgressBar.fillAmount = progress;
        _loadingText.SetText($"{Mathf.RoundToInt(progress * 100)}%");
    }
}
