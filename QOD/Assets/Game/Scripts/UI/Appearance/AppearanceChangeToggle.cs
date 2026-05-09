using System;
using UnityEngine;
using UnityEngine.UI;

public class AppearanceChangeToggle : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private Toggle _toggle;

    public Toggle Toggle => _toggle;

    public void Init(Sprite sprite, Color color, Action onClick)
    {
        _image.sprite = sprite;
        _image.color = color;
        _toggle.interactable = false;
        _toggle.onValueChanged.AddListener(isOn => { if (isOn) onClick?.Invoke(); });
    }

    public void ResetButton()
    {
        _toggle.onValueChanged.RemoveAllListeners();
    }
}
