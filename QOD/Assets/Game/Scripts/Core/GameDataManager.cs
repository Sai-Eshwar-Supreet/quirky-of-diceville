using UnityEngine;
using System.Collections.Generic;

public class GameDataManager : MonoSingleton<GameDataManager>
{
    [SerializeField] private Color _platformColor = Color.white;
    [SerializeField] private List<Color> _diceColors;

    [SerializeField] private Texture2D _platformTexture;
    [SerializeField] private List<Texture2D> _diceValueTextures;
    public Color PlatformColor => _platformColor;
    public Texture2D PlatformTexture => _platformTexture;

    public Color GetColorForDice(int colorIndex)
    {
        if (colorIndex < 0 || colorIndex >= _diceColors.Count) return PlatformColor;
        return _diceColors[colorIndex];
    } 

    public Texture2D GetValueTextureForDice(int valueIndex)
    {
        if (valueIndex < 0 || valueIndex >= _diceValueTextures.Count) return PlatformTexture;
        return _diceValueTextures[valueIndex];
    }
}
