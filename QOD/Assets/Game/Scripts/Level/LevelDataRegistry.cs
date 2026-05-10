using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelDataRegistry", menuName = "Levels/DataRegistry")]
public class LevelDataRegistry : ScriptableObject
{
    [SerializeField] private LevelData[] _levelData;

    public IReadOnlyList<LevelData> LevelDataList => _levelData;
}
