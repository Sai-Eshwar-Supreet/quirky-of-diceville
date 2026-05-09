using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ToggleGroup))]
public class AppearanceChangeToggleGroup : MonoBehaviour
{
    [SerializeField] private ToggleGroup _toggleGroup;

    private readonly Dictionary<int, AppearanceChangeToggle> _toggles = new ();

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
