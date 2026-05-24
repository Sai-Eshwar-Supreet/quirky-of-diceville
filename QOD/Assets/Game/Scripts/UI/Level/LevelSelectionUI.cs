using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Schema;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionUI : MonoBehaviour
{
    [SerializeField] private Button _openButton;
    [SerializeField] private Button _closeButton;
    [SerializeField] private GameObject _levelSelectionPanel;
    [SerializeField] private RectTransform _levelUIContainer;
    [SerializeField] private GridLayoutGroup _levelLayoutGroup;
    [SerializeField] private LevelUI _levelUIPrefab;

    private readonly Dictionary<int, LevelUI> _levelUIs = new();

    public event Action<int> OnLevelPlayRequested;

    private int _selectedLevel = -1;

    public bool IsOpen => _levelSelectionPanel.activeInHierarchy;

    private void Awake()
    {
        _openButton.onClick.AddListener(Open);
        _closeButton.onClick.AddListener(Close);
    }

    private void OnDestroy()
    {
        if(_openButton != null) _openButton.onClick.RemoveListener(Open);
        if(_closeButton != null) _closeButton.onClick.RemoveListener(Close);
    }

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

        SetupNavigation();
    }

    private void SetupNavigation()
    {
        for (int i = 0; i < _levelUIs.Count; i++)
        {
            var current = _levelUIs[i];
            var left = GetElementInDirction(Vector2Int.left, i);
            var right = GetElementInDirction(Vector2Int.right, i);
            var up = GetElementInDirction(Vector2Int.down, i);
            var down = GetElementInDirction(Vector2Int.up, i);

            current.SetupNavigation(left, right, up, down);
        }
    }

    private LevelUI GetElementInDirction(Vector2Int direction, int currentIndex)
    {
        if(currentIndex < 0 || currentIndex  >= _levelUIs.Count) return null;

        int constraint = _levelLayoutGroup.constraintCount;


        if(direction.x != 0)
        {
            int row = Mathf.FloorToInt(currentIndex / _levelLayoutGroup.constraintCount);
            int rowStartIndex = _levelLayoutGroup.constraintCount * row;
            int count = (Mathf.Clamp(rowStartIndex + _levelLayoutGroup.constraintCount, 0, _levelUIs.Count)  - rowStartIndex);

            int relativeCurrentIndex = currentIndex - rowStartIndex;

            for(int i = 1; i <= constraint; i++)
            {
                int relativeNextElementIndex = (count + relativeCurrentIndex + (i * direction.x)) % count;
                LevelUI element = _levelUIs[rowStartIndex + relativeNextElementIndex];
                if (element != null && element.IsInteractable) return element;

            }
        }
        else
        {
            var maxChecks = Mathf.CeilToInt((float)_levelUIs.Count / _levelLayoutGroup.constraintCount);
            var total = maxChecks * _levelLayoutGroup.constraintCount;

            for(int i = 1; i <= maxChecks; i++)
            {
                int nextElementIndex = (total + currentIndex + (i * direction.y * _levelLayoutGroup.constraintCount)) % total;

                if(nextElementIndex >= _levelUIs.Count) continue;

                LevelUI element = _levelUIs[nextElementIndex];
                if (element != null && element.IsInteractable) return element;
            }
        }

        return null;
    }

    public void SetCurrentLevel(int currentLevel)
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

            SetupNavigation();
        }
    }

    public void Open()
    {
        _levelSelectionPanel.SetActive(true);
    }

    public void Close()
    {
        _levelSelectionPanel.SetActive(false);
    }
}
