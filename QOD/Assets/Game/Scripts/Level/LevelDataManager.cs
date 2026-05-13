using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

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

    public void Reset(IReadOnlyList<int> unlockedColors, IReadOnlyList<int> unlockedValues)
    {
        _unlockedColors.Clear();
        _unlockedValues.Clear();

        foreach (int colorId in unlockedColors)
        {
            _unlockedColors.Add(colorId);
        }

        foreach (int valueId in unlockedValues)
        {
            _unlockedValues.Add(valueId);
        }
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
