using UnityEngine;

[CreateAssetMenu(fileName = "DiceValueData", menuName = "Dice/ValueData")]
public class DiceValueData : ScriptableObject
{
    [SerializeField] private int id;
    [SerializeField] private Texture2D texture;

    public int ID => id;
    public Texture2D Texture => texture;
}
