using UnityEngine;

[CreateAssetMenu(fileName = "LevelDataRegistry", menuName = "Levels/DataRegistry")]
public class LevelDataRegistry : ScriptableObject
{
    [SerializeField] private LevelData[] _levelData;

    public LevelData[] LevelDataList => _levelData;
}
