using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectionUI : MonoBehaviour
{
    [SerializeField] private GameObject _levelSelectionPanel;
    [SerializeField] private RectTransform _levelUIContainer;
    [SerializeField] private LevelUI _levelUIPrefab;

    private readonly Dictionary<int, LevelUI> _levelUIs = new();

    public event Action<int> OnLevelPlayRequested;

    private int _selectedLevel = -1;

    public bool IsOpen => _levelSelectionPanel.activeInHierarchy;

    public void Init(int selectedLevel)
    {
        _selectedLevel = selectedLevel;
        var gameDataManager = ServiceLocator.Get<GameDataManager>();

        foreach(var levelData in gameDataManager.LevelDataList)
        {
            var levelUI = Instantiate(_levelUIPrefab, _levelUIContainer);
            levelUI.SetInteractable(_selectedLevel != levelData.ID);
            levelUI.Set(levelData.ID + 1, () => OnLevelPlayRequested?.Invoke(levelData.ID));

            _levelUIs.Add(levelData.ID, levelUI);
        }
    }

    public void Open(int currentLevel)
    {
        if(_selectedLevel != currentLevel)
        {
            if (_levelUIs.TryGetValue(_selectedLevel, out var levelUI))
            {
                levelUI.SetInteractable(true);
            }

            _selectedLevel = currentLevel;
            if (_levelUIs.TryGetValue(_selectedLevel, out levelUI))
            {
                levelUI.SetInteractable(false);
            }
        }

        var level = ServiceLocator.Get<Level>();
        if(level != null) level.Pause(true);

        _levelSelectionPanel.SetActive(true);
    }

    public void Close()
    {
        _levelSelectionPanel.SetActive(false);

        var level = ServiceLocator.Get<Level>();
        if (level != null) level.Pause(false);
    }
}
