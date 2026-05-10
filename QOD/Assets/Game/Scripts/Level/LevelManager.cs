using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[DefaultExecutionOrder(-50)]
public class LevelManager : MonoSingleton<LevelManager>
{
    [SerializeField] private Button _exitToMenuButton;
    [SerializeField] private LevelSelectionUI _levelSelectionUI;

    private LevelDataManager _levelDataManager;
    private int _currentLevel = -1;
    private Level _levelObject;

    protected override void Awake()
    {
        base.Awake();
        _levelDataManager = new LevelDataManager();
    }

    private void OnEnable()
    {
        ServiceLocator.Register<ILevelDataQueryService>(_levelDataManager);
        ServiceLocator.Register<ILevelDataUpdateService>(_levelDataManager);

        _levelSelectionUI.OnLevelPlayRequested += LoadLevel;
        _exitToMenuButton.onClick.AddListener(ExitLevel);

        LoadLevel(0);
        _levelSelectionUI.Init(_currentLevel);
    }

    private void OnDisable()
    {
        _levelSelectionUI.OnLevelPlayRequested -= LoadLevel;
        _exitToMenuButton.onClick.RemoveListener(ExitLevel);

        ServiceLocator.Unregister<ILevelDataQueryService>();
        ServiceLocator.Unregister<ILevelDataUpdateService>();
    }

    public void LoadLevel(int levelId)
    {
        if (_currentLevel == levelId) return;

        UnloadLevel();

        _levelDataManager.Reset();
        _currentLevel = levelId;
        _levelObject = Instantiate(ServiceLocator.Get<GameDataManager>().GetLevelPrefab(levelId));
        ServiceLocator.Register(_levelObject);
        _levelSelectionUI.Close();
    }

    public void UnloadLevel()
    {
        if(_levelObject != null )
        {
            ServiceLocator.Unregister<Level>();
            Destroy(_levelObject.gameObject);
            _levelObject = null;
        }
    }

    public void GoToNextLevel()
    {
        var gameDataManager = ServiceLocator.Get<GameDataManager>();

        var nextLevelId = gameDataManager.GetNextLevelId(_currentLevel);

        LoadLevel(nextLevelId);
    }

    public void TogglePause(InputAction.CallbackContext ctx)
    {
        if (ctx.phase != InputActionPhase.Performed) return;
        if (_levelSelectionUI.IsOpen) _levelSelectionUI.Close();
        else _levelSelectionUI.Open(_currentLevel);
    }

    public void ExitLevel()
    {
        UnloadLevel();
        ServiceLocator.Get<LoadingManger>().Load(SceneType.Menu);
    }
}
