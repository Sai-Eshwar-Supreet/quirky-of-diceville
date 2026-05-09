using UnityEngine;

[CreateAssetMenu(fileName = "DiceValueData", menuName = "Dice/ValueData")]
public class DiceValueData : ScriptableObject
{
    [SerializeField] private int _id;
    [SerializeField] private Texture2D _texture;
    [SerializeField] private Sprite _sprite;

    public int ID => _id;
    public Texture2D Texture => _texture;
    public Sprite Sprite => _sprite;
}
