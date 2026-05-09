using UnityEngine;

[System.Serializable]
public class LevelData
{
    [SerializeField] private int _id;
    [SerializeField] private Level _levelPrefab;
    [SerializeField] private Sprite _sprite;
    
    public int ID => _id;
    public Level LevelPrefab => _levelPrefab;
    public Sprite Sprite => _sprite;
}
