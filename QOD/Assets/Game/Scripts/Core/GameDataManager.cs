using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using Mono.Cecil.Cil;

[DefaultExecutionOrder(-1)]
public class  GameDataManager : MonoBehaviour
{
    [SerializeField] private DiceDataRegistry _diceDataRegistry;
    [SerializeField] private LevelDataRegistry _levelDataRegistry;
    public Color DefaultColor { get; private set; }
    public Texture2D DefaultTexture { get; private set;  }

    private readonly Dictionary<int, Color> _colorCache = new();
    private readonly Dictionary<int, Texture2D> _textureCache = new();
    private readonly Dictionary<int, Level> _levelCache = new();

    public DiceColorData[] ColorDataList => _diceDataRegistry.ColorDataList;
    public DiceValueData[] ValueDataList => _diceDataRegistry.ValueDataList;
    public LevelData[] LevelDataList => _levelDataRegistry.LevelDataList;

    public void Load()
    {
        _colorCache.Clear();
        _textureCache.Clear();

        DefaultColor = ColorDataList[0].Color;
        foreach (var colorData in ColorDataList)
        {
            _colorCache.Add(colorData.ID, colorData.Color);
        }

        DefaultTexture = ValueDataList[0].Texture;
        foreach (var valueData in ValueDataList)
        {
            _textureCache.Add(valueData.ID, valueData.Texture);
        }

        _levelCache.Clear();
        foreach(var levelData in _levelDataRegistry.LevelDataList)
        {
            _levelCache.Add(levelData.ID, levelData.LevelPrefab);
        }
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

    public Level GetLevelPrefab(int levelIndex)
    {
        if(_levelCache.TryGetValue(levelIndex, out var levelPrefab))
        {
            return levelPrefab;
        }

        return null;
    }

    public int GetNextLevelId(int currentLevel)
    {
        var keys = _levelCache.Keys.ToList();

        var index = keys.IndexOf(currentLevel);

        var nextIndex = (keys.Count + index + 1) % keys.Count;

        return keys[nextIndex];
    }
}
