using UnityEngine;
using System.Collections.Generic;

[DefaultExecutionOrder(-1)]
public class GameDataManager : MonoSingleton<GameDataManager>
{
    [SerializeField] private DiceDataRegistry _diceDataRegistry;

    [SerializeField] private Color _platformColor = Color.white;
    [SerializeField] private Texture2D _platformTexture;
    public Color PlatformColor => _platformColor;
    public Texture2D PlatformTexture => _platformTexture;

    public Color GetColorForDice(int colorIndex)
    {
        if(_diceDataRegistry.TryGetDiceColor(colorIndex, out var color)) {
            return color;
        }
        return PlatformColor;
    }

    public Texture2D GetValueTextureForDice(int valueIndex)
    {
        if(_diceDataRegistry.TryGetDiceTexture(valueIndex, out var texture)) {
            return texture;
        }
        return PlatformTexture;
    }
}
