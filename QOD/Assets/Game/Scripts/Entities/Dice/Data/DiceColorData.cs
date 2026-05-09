using UnityEngine;

[CreateAssetMenu(fileName = "DiceColorData", menuName = "Dice/ColorData")]
public class DiceColorData : ScriptableObject
{
    [SerializeField] private int _id;
    [SerializeField] private Color _color;

    public int ID => _id;
    public Color Color => _color;
}
