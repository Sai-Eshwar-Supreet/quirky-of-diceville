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
    [SerializeField] private int _defaultColorIndex = 0;
    [SerializeField] private int _defaultValueIndex = 0;
    [SerializeField] private MeshRenderer _diceRenderer;
    private Material _material;

    public DiceType Type => _type;
    public int ColorIndex { get; private set;  }
    public int ValueIndex { get; private set;  }

    public void Init()
    {
        if (_material) return;

        _material = new Material(_diceRenderer.material)
        {
            name = $"{gameObject.name} material"
        };
        _diceRenderer.material = _material;

        var _gameDataManager = ServiceLocator.Get<GameDataManager>();

        ColorIndex = _defaultColorIndex;
        ValueIndex = _defaultValueIndex;

        _material.SetColor("_BaseColor", _gameDataManager.GetColorForDice(ColorIndex));
        _material.SetTexture("_BaseMap", _gameDataManager.GetValueTextureForDice(ValueIndex));
    }

    public void SetColor(int colorIndex)
    {
        if(!_type.HasFlag(DiceType.Color)) return;

        ColorIndex = colorIndex;
        var color = ServiceLocator.Get<GameDataManager>().GetColorForDice(ColorIndex);
        _material.SetColor("_BaseColor", color);
    }

    public void SetValue(int valueIndex)
    {
        if (!_type.HasFlag(DiceType.Value)) return;

        ValueIndex = valueIndex;
        var tex2D = ServiceLocator.Get<GameDataManager>().GetValueTextureForDice(ValueIndex);
        _material.SetTexture("_BaseMap", tex2D);
    }

    public void ResetDice()
    {
        
        SetColor(_defaultColorIndex);
        SetValue(_defaultValueIndex);
    }
}
