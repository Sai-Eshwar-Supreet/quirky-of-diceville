using UnityEngine;

[CreateAssetMenu(fileName = "DiceColorData", menuName = "Dice/ColorData")]
public class DiceColorData : ScriptableObject
{
    [SerializeField] private int id;
    [SerializeField] private Color color;

    public int ID => id;
    public Color Color => color;
}
