using System.Drawing;
using UnityEngine;

[System.Flags]
public enum DiceType
{
    None = 0,
    Color = 1 << 0,
    Value = 1 << 1
}
public class DiceStateManager : MonoBehaviour
{
    [SerializeField] private DiceType _type = DiceType.Color | DiceType.Value;
    [SerializeField] private int _startColorIndex = 0;
    [SerializeField] private int _startValueIndex = 0;
    [SerializeField] private DiceVisual _renderer;

    public bool IsEnabled { get; private set; }

    public DiceType Type => _type;
    public int ColorIndex { get; private set;  }
    public int ValueIndex { get; private set;  }

    public void Enable()
    {
        if (IsEnabled) return;
        IsEnabled = true;
        var _gameDataManager = ServiceLocator.Get<GameDataManager>();

        ColorIndex = _startColorIndex;
        ValueIndex = _startValueIndex;

        _renderer.SetColor(_gameDataManager.GetColorForDice(ColorIndex));
        _renderer.SetTexture(_gameDataManager.GetValueTextureForDice(ValueIndex));
    }

    public bool Matches(DiceStateManager other)
    {
        bool isColorMatch = !_type.HasFlag(DiceType.Color) || other.ColorIndex == ColorIndex;
        bool isValueMatch = !_type.HasFlag(DiceType.Value) || other.ValueIndex == ValueIndex;

        bool isMatch = isColorMatch && isValueMatch;
        return isMatch;
    }

    public void SetColor(int colorIndex)
    {
        if(!IsEnabled || !_type.HasFlag(DiceType.Color)) return;

        ColorIndex = colorIndex;
        var color = ServiceLocator.Get<GameDataManager>().GetColorForDice(ColorIndex);
        _renderer.SetColor(color);
    }

    public void SetValue(int valueIndex)
    {
        if (!IsEnabled || !_type.HasFlag(DiceType.Value)) return;

        ValueIndex = valueIndex;
        var tex2D = ServiceLocator.Get<GameDataManager>().GetValueTextureForDice(ValueIndex);
        _renderer.SetTexture(tex2D);
    }

    public void Disable()
    {
        if (!IsEnabled) return;
        IsEnabled = false;
        _renderer.ResetOverrides();
    }
}
