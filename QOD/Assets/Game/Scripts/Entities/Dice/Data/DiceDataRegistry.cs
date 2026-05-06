using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DiceDataRegistry", menuName = "Dice/DataRegistry")]
public class DiceDataRegistry : ScriptableObject
{
    [SerializeField] private DiceValueData[] _valueData;
    [SerializeField] private DiceColorData[] _colorData;

    public int ValueDataCount => _valueData.Length;
    public int ColorDataCount => _colorData.Length;

    public bool TryGetDiceTexture(int valueId, out Texture2D texture)
    {
        foreach (var data in _valueData)
        {
            if (data.ID == valueId)
            {
                texture = data.Texture;
                return true;
            }
        }
        texture = null;
        return false;
    }

    public bool TryGetDiceColor(int colorId, out Color color)
    {
        foreach (var data in _colorData)
        {
            if (data.ID == colorId)
            {
                color = data.Color;
                return true;
            }
        }
        color = default;
        return false;
    }
}
