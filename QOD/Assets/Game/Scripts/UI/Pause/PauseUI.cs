using UnityEngine;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private Button _continueButton;

    private void Awake()
    {
        _continueButton.onClick.AddListener(Close);
    }

    private void OnDestroy()
    {
        if(_continueButton != null) _continueButton.onClick.RemoveListener(Close);
    }
    public bool IsOpen => _pausePanel.activeInHierarchy;

    public void Open()
    {

        var level = ServiceLocator.Get<Level>();
        if (level != null) level.Pause(true);

        _pausePanel.SetActive(true);
    }

    public void Close()
    {
        _pausePanel.SetActive(false);

        var level = ServiceLocator.Get<Level>();
        if (level != null) level.Pause(false);
    }
}
