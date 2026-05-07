using UnityEngine;

public class DiceStateManager : MonoBehaviour
{
    [SerializeField] private int _colorIndex = 0;
    [SerializeField] private int _valueIndex = 0;
    [SerializeField] private MeshRenderer _diceRenderer;
    
    private Material _material;
    private Material Material
    {
        get
        {
            if (_material == null) InitRenderer();
            return _material;
        }
    }

    public int ColorIndex => _colorIndex;
    public int ValueIndex => _valueIndex;

    private void Awake()
    {
        if (_material == null) InitRenderer();
    }

    private void InitRenderer()
    {
        _material = new Material(_diceRenderer.material)
        {
            name = $"{gameObject.name} material"
        };
        _diceRenderer.material = _material;

        SetColor(_colorIndex);
        SetValue(_valueIndex);
    }

    public void SetColor(int colorIndex)
    {
        _colorIndex = colorIndex;
        var color = ServiceLocator.Get<GameDataManager>().GetColorForDice(_colorIndex);
        Material.SetColor("_BaseColor", color);
    }

    public void SetValue(int valueIndex)
    {
        _valueIndex = valueIndex;
        var tex2D = ServiceLocator.Get<GameDataManager>().GetValueTextureForDice(_valueIndex);
        Material.SetTexture("_BaseMap", tex2D);
    }

    public void ResetDice()
    {
        _colorIndex = -1;
        _valueIndex = -1;

        var _gameDataManager = ServiceLocator.Get<GameDataManager>();

        Material.SetColor("_BaseColor", _gameDataManager.DefaultColor);
        Material.SetTexture("_BaseMap", _gameDataManager.DefaultTexture);
    }
}
