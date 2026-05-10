using System;
using System.Collections.Generic;

public class LevelDataManager : ILevelDataQueryService, ILevelDataUpdateService
{
    public event Action OnAppearanceUnlocked;
    public event Action<int> OnColorUnlocked;
    public event Action<int> OnValueUnlocked;

    private readonly HashSet<int> _unlockedColors;
    private readonly HashSet<int> _unlockedValues;

    public LevelDataManager()
    {
        _unlockedColors = new ();
        _unlockedValues = new ();
    }

    public void Reset()
    {
        _unlockedColors.Clear();
        _unlockedValues.Clear();

        _unlockedColors.Add(0); // Default color
        _unlockedValues.Add(0); // Default value
    }

    public bool IsColorUnlocked(int colorId) => _unlockedColors.Contains(colorId);
    public bool IsValueUnlocked(int valueId) => _unlockedValues.Contains(valueId);

    public void UnlockValue(int valueId)
    {
        if (_unlockedValues.Contains(valueId)) return;
        _unlockedValues.Add(valueId);
        OnValueUnlocked?.Invoke(valueId);
        OnAppearanceUnlocked?.Invoke();
    }

    public void UnlockColor(int colorId)
    {
        if (_unlockedColors.Contains(colorId)) return;
        _unlockedColors.Add(colorId);
        OnColorUnlocked?.Invoke(colorId);
        OnAppearanceUnlocked?.Invoke();
    }
}
