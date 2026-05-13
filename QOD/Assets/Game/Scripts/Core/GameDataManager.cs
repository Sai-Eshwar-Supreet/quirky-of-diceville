using System;
using System.Collections.Generic;
using UnityEngine;

public class  GameDataManager : MonoBehaviour
{
    [SerializeField] private DiceDataRegistry _diceDataRegistry;
    [SerializeField] private LevelDataRegistry _levelDataRegistry;

    private readonly Dictionary<int, DiceColorData> _colorCache = new();
    private readonly Dictionary<int, DiceValueData> _textureCache = new();
    private readonly Dictionary<int, LevelData> _levelCache = new();

    public IReadOnlyList<DiceColorData> ColorDataList => _diceDataRegistry.ColorDataList;
    public IReadOnlyList<DiceValueData> ValueDataList => _diceDataRegistry.ValueDataList;
    public IReadOnlyList<LevelData> LevelDataList => _levelDataRegistry.LevelDataList;

    public void Load()
    {
        _colorCache.Clear();
        _textureCache.Clear();
        _levelCache.Clear();

        foreach (var colorData in ColorDataList)
        {
            if(!_colorCache.TryAdd(colorData.ID, colorData))
            {
                Debug.LogError($"Duplicate color ID: {colorData.ID}");
            }
        }

        foreach (var valueData in ValueDataList)
        {
            if (!_textureCache.TryAdd(valueData.ID, valueData))
            {
                Debug.LogError($"Duplicate value ID: {valueData.ID}");
            }
        }

        foreach(var levelData in _levelDataRegistry.LevelDataList)
        {
            if (!_levelCache.TryAdd(levelData.ID, levelData))
            {
                Debug.LogError($"Duplicate level ID: {levelData.ID}");
            }
        }
    }

    public Color GetColorForDice(int colorId)
    {
        if (_colorCache.TryGetValue(colorId, out var data))
        {
            return data.Color;

        }

        throw new ArgumentException($"Invalid level id {colorId}", nameof(colorId));
    }

    public Texture2D GetValueTextureForDice(int valueId)
    {
        if (_textureCache.TryGetValue(valueId, out var data))
        {
            return data.Texture;
        }

        throw new ArgumentException($"Invalid value id {valueId}", nameof(valueId));
    }

    public LevelData GetLevelData(int levelId)
    {
        if(_levelCache.TryGetValue(levelId, out var data))
        {
            return data;
        }

        throw new ArgumentException($"Invalid level id {levelId}", nameof(levelId));
    }

    public int GetNextLevelId(int currentLevelId)
    {
        if (_levelCache.TryGetValue(currentLevelId, out var data))
        {
            return data.NextLevelId;
        }

        throw new ArgumentException( $"Invalid level id {currentLevelId}", nameof(currentLevelId));
    }
}
