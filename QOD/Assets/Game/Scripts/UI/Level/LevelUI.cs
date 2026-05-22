using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _level;
    [SerializeField] private Button _playLevelButton;

    private Action _onPlayCallback;

    private void Awake()
    {
        _playLevelButton.onClick.AddListener(() => _onPlayCallback?.Invoke());
    }

    public void Set(int level, Action onPlayCallback)
    {
        _level.SetText(level.ToString("D2"));
        _onPlayCallback = onPlayCallback;
    }

    public void SetInteractable(bool interactable)
    {
        _playLevelButton.interactable = interactable;
    }
}
