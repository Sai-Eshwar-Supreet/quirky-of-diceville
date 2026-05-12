using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private ToggleGroup _toggleGroup;
    [SerializeField] private Toggle _itemPrefab;
    [SerializeField] private RectTransform _container;

    private readonly Dictionary<int, Toggle> _playerUI = new();

    public void AddPlayer(int playerIndex)
    {
        var item = Instantiate(_itemPrefab, _container);
        item.group = _toggleGroup;
        _playerUI.Add(playerIndex, item);
    }

    public void SwitchTo(int playerIndex)
    {
        if(_playerUI.TryGetValue(playerIndex, out Toggle item))
        {
            item.isOn = true;
        }
    }
}
