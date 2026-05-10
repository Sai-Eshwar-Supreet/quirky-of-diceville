using System;
using UnityEngine;

public interface ILevelDataQueryService
{
    public event Action OnAppearanceUnlocked;
    public event Action<int> OnColorUnlocked;
    public event Action<int> OnValueUnlocked;

    public bool IsColorUnlocked(int colorId);
    public bool IsValueUnlocked(int valueId);
}
