using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[DefaultExecutionOrder(-1)]
public class GameDataManager : MonoBehaviour
{
    [SerializeField] private DiceDataRegistry _diceDataRegistry;
    public Color DefaultColor { get; private set; }
    public Texture2D DefaultTexture { get; private set;  }

    private readonly Dictionary<int, Color> _colorCache = new();
    private readonly Dictionary<int, Texture2D> _textureCache = new();

    private readonly HashSet<int> _unlockedColors = new();
    private readonly HashSet<int> _unlockedValues = new();

    public void Load()
    {
        _colorCache.Clear();
        _textureCache.Clear();

        DefaultColor = _diceDataRegistry.ColorDataList[0].Color;
        foreach (var colorData in _diceDataRegistry.ColorDataList)
        {
            _colorCache.Add(colorData.ID, colorData.Color);
        }

        DefaultTexture = _diceDataRegistry.ValueDataList[0].Texture;
        foreach (var valueData in _diceDataRegistry.ValueDataList)
        {
            _textureCache.Add(valueData.ID, valueData.Texture);
        }

        ResetLevelData();
    }

    public void ResetLevelData()
    {
        _unlockedColors.Clear();
        _unlockedValues.Clear();

        _unlockedColors.Add(0); // Default color
        _unlockedValues.Add(0); // Default value
    }

    public bool IsColorUnlocked(int id) => _unlockedColors.Contains(id);
    public bool IsValueUnlocked(int id) => _unlockedValues.Contains(id);

    public void UnlockValue(int id)
    {
        if(_unlockedValues.Contains(id)) return;
        _unlockedValues.Add(id);
    }

    public void UnlockColor(int id)
    {
        if(_unlockedColors.Contains(id)) return;
        _unlockedColors.Add(id);
    }

    public Color GetUnlockedColorForDice(int colorIndex)
    {
        if (_unlockedColors.Contains(colorIndex) && _colorCache.TryGetValue(colorIndex, out var color))
        {
            return color;
        }
        return DefaultColor;
    }

    public Texture2D GetUnlockedValueTextureForDice(int valueIndex)
    {
        if (_unlockedValues.Contains(valueIndex) && _textureCache.TryGetValue(valueIndex, out var texture))
        {
            return texture;
        }
        return DefaultTexture;
    }

    public Color GetColorForDice(int colorIndex)
    {
        if (_colorCache.TryGetValue(colorIndex, out var color))
        {
            return color;

        }
        return DefaultColor;
    }

    public Texture2D GetValueTextureForDice(int valueIndex)
    {
        if (_textureCache.TryGetValue(valueIndex, out var texture))
        {
            return texture;
        }
        return DefaultTexture;
    }

    public List<int> _col;
    public List<int> _val;
    private void Update()
    {
        _col = _unlockedColors.ToList();
        _val = _unlockedValues.ToList();
    }
}
