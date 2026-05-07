using UnityEngine;
using System.Collections.Generic;

[DefaultExecutionOrder(-1)]
public class GameDataManager : MonoBehaviour
{
    [SerializeField] private DiceDataRegistry _diceDataRegistry;

    [SerializeField] private Color _defaultColor = Color.white;
    [SerializeField] private Texture2D _defaultTexture;
    public Color DefaultColor => _defaultColor;
    public Texture2D DefaultTexture => _defaultTexture;

    private Dictionary<int, Color> _colorCache = new ();
    private Dictionary<int, Texture2D> _textureCache = new ();

    public void Load()
    {
        _colorCache.Clear ();
        _textureCache.Clear ();

        
        foreach (var colorData in _diceDataRegistry.ColorDataList) {
            _colorCache.Add(colorData.ID, colorData.Color);
        }

        foreach (var valueData in _diceDataRegistry.ValueDataList) {
            _textureCache.Add(valueData.ID, valueData.Texture);
        }
    }

    public Color GetColorForDice(int colorIndex)
    {
        if(_colorCache.TryGetValue(colorIndex, out var color)) {
            return color;
        }
        return DefaultColor;
    }

    public Texture2D GetValueTextureForDice(int valueIndex)
    {
        if(_textureCache.TryGetValue(valueIndex, out var texture)) {
            return texture;
        }
        return DefaultTexture;
    }
}
