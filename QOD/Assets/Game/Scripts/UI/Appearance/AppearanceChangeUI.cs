using UnityEngine;

public class AppearanceChangeUI : MonoBehaviour
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private AppearanceChangeToggleGroup _colorToggleGroup;
    [SerializeField] private AppearanceChangeToggleGroup _valueToggleGroup;
    [SerializeField] private AppearanceChangeToggle _togglePrefab;

    public event System.Action<int> OnColorSelected;
    public event System.Action<int> OnValueSelected;

    public bool IsOpen => _panel.activeInHierarchy;

    private void Awake()
    {
        var gameDataManager = ServiceLocator.Get<GameDataManager>();
        foreach (var colorData in gameDataManager.ColorDataList)
        {
            var toggle = Instantiate(_togglePrefab);
            toggle.Set(null, colorData.Color, () => { OnColorSelected?.Invoke(colorData.ID); });
            _colorToggleGroup.Add(colorData.ID, toggle);
        }
        foreach (var valueData in gameDataManager.ValueDataList)
        {
            var toggle = Instantiate(_togglePrefab);
            toggle.Set(valueData.Sprite, Color.white, () => { OnValueSelected?.Invoke(valueData.ID); });
            _valueToggleGroup.Add(valueData.ID, toggle);
        }

        UpdateUnlocks();
    }

    public void UpdateUnlocks()
    {
        var gameDataManager = ServiceLocator.Get<GameDataManager>();
        var levelDataQueryService = ServiceLocator.Get<ILevelDataQueryService>();
        foreach (var colorData in gameDataManager.ColorDataList)
        {
            var isUnlocked = levelDataQueryService.IsColorUnlocked(colorData.ID);
            _colorToggleGroup.SetInteractable(colorData.ID, isUnlocked);
        }
        foreach (var valueData in gameDataManager.ValueDataList)
        {
            var isUnlocked = levelDataQueryService.IsValueUnlocked(valueData.ID);
            _valueToggleGroup.SetInteractable(valueData.ID, isUnlocked);
        }
    }

    public void Open(int activeColorIndex, int activeValueIndex)
    {
        _panel.SetActive(true);
        _colorToggleGroup.Select(activeColorIndex);
        _valueToggleGroup.Select(activeValueIndex);
    }

    public void Close()
    {
        _panel.SetActive(false);
    }
}
