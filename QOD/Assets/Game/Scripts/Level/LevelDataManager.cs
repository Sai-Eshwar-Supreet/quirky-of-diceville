using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelDataManager : MonoBehaviour
{
    public event Action OnAppearanceUnlocked;
    public event Action<int> OnColorUnlocked;
    public event Action<int> OnValueUnlocked;

    private readonly HashSet<int> _unlockedColors = new();
    private readonly HashSet<int> _unlockedValues = new();
    public bool IsColorUnlocked(int id) => _unlockedColors.Contains(id);
    public bool IsValueUnlocked(int id) => _unlockedValues.Contains(id);

    public void Init()
    {
        _unlockedColors.Clear();
        _unlockedValues.Clear();

        _unlockedColors.Add(0); // Default color
        _unlockedValues.Add(0); // Default value
    }

    public void UnlockValue(int id)
    {
        if (_unlockedValues.Contains(id)) return;
        _unlockedValues.Add(id);
        OnValueUnlocked?.Invoke(id);
        OnAppearanceUnlocked?.Invoke();
    }

    public void UnlockColor(int id)
    {
        if (_unlockedColors.Contains(id)) return;
        _unlockedColors.Add(id);
        OnColorUnlocked?.Invoke(id);
        OnAppearanceUnlocked?.Invoke();
    }
}
