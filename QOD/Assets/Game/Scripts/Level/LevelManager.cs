using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[DefaultExecutionOrder(-50)]
public class LevelManager : MonoSingleton<LevelManager>
{
    [SerializeField] private Button _exitToMenuButton;
    [SerializeField] private PauseUI _pauseUI;
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

        // switch on loading canvas
        UnloadLevel();

        //fake load the level

        var levelData = ServiceLocator.Get<GameDataManager>().GetLevelData(levelId);

        _levelDataManager.Reset(levelData.DefaultUnlockedColors, levelData.DefaultUnlockedValues);  
        _currentLevel = levelId;
        _levelObject = Instantiate(levelData.LevelPrefab);
        ServiceLocator.Register(_levelObject);
        _levelSelectionUI.Close();
        _pauseUI.Close();
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

    public async Task GoToNextLevel()
    {
        if (_levelObject != null) _levelObject.Pause(true);

        var gameDataManager = ServiceLocator.Get<GameDataManager>();

        var nextLevelId = gameDataManager.GetNextLevelId(_currentLevel);

        await Task.Delay(500); // delay

        // show level completion ui for x duration

        LoadLevel(nextLevelId);
    }

    public void TogglePause(InputAction.CallbackContext ctx)
    {
        if (ctx.phase != InputActionPhase.Performed) return;
        if (_pauseUI.IsOpen)
        {
            _levelSelectionUI.Close();
            _pauseUI.Close();
        }
        else
        {
            _pauseUI.Open();
            _levelSelectionUI.SetCurrentLevel(_currentLevel);
        }
    }

    public void ExitLevel()
    {
        UnloadLevel();
        ServiceLocator.Get<LoadingManger>().Load(SceneType.Menu);
    }

    private void OnApplicationFocus(bool focus)
    {
        if (!focus) _pauseUI.Open();
    }
}
