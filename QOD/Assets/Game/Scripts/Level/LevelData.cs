using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Levels/Data")]
public class LevelData : ScriptableObject
{
    [SerializeField] private int _id;
    [SerializeField] private LevelData _nextLevel;
    [SerializeField] private Level _levelPrefab;
    [SerializeField] private Sprite _sprite;
    [SerializeField] private List<DiceColorData> _defaultUnlockedColors;
    [SerializeField] private List<DiceValueData> _defaultUnlockedValues;

    private List<int> _defaultUnlockedColorsCache;
    private List<int> _defaultUnlockedValuesCache;

    public int ID => _id;
    public int NextLevelId => _nextLevel.ID;
    public Level LevelPrefab => _levelPrefab;
    public Sprite Sprite => _sprite;
    public IReadOnlyList<int> DefaultUnlockedColors
    {
        get
        {
            if(_defaultUnlockedColorsCache == null || _defaultUnlockedColorsCache.Count != _defaultUnlockedColors.Count)
            {
                _defaultUnlockedColorsCache = _defaultUnlockedColors.Select(data => data.ID).ToList();
            }

            return _defaultUnlockedColorsCache;
        }
    }
    public IReadOnlyList<int> DefaultUnlockedValues
    {
        get
        {
            if (_defaultUnlockedValuesCache == null || _defaultUnlockedValuesCache.Count != _defaultUnlockedValues.Count)
            {
                _defaultUnlockedValuesCache = _defaultUnlockedValues.Select(data => data.ID).ToList();
            }

            return _defaultUnlockedValuesCache;
        }
    }
}
