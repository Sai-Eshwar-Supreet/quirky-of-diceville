using System;
using UnityEngine;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour
{
    [SerializeField] private Image _levelImage;
    [SerializeField] private Button _playLevelButton;

    private Action _onPlayCallback;

    private void Awake()
    {
        _playLevelButton.onClick.AddListener(() => _onPlayCallback?.Invoke());
    }

    public void Set(Sprite icon, Action onPlayCallback)
    {
        _levelImage.sprite = icon;
        _onPlayCallback = onPlayCallback;
    }

    public void SetInteractable(bool interactable)
    {
        _playLevelButton.interactable = interactable;
    }
}
