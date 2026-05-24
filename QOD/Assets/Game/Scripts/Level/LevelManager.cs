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

    private PlayerUIInput.PlayerUIActions _uiActions;

    protected override void Awake()
    {
        base.Awake();
        _levelDataManager = new LevelDataManager();
        _uiActions = new PlayerUIInput().PlayerUI;
    }

    private void OnEnable()
    {
        ServiceLocator.Register<ILevelDataQueryService>(_levelDataManager);
        ServiceLocator.Register<ILevelDataUpdateService>(_levelDataManager);

        _levelSelectionUI.OnLevelPlayRequested += LoadLevel;
        _levelSelectionUI.OnOpen += PauseLevel;
        _levelSelectionUI.OnClose += ResumeLevel;
        _pauseUI.OnOpen += PauseLevel;
        _pauseUI.OnClose += ResumeLevel;
        _exitToMenuButton.onClick.AddListener(ExitLevel);

        LoadLevel(0);
        _levelSelectionUI.Init(_currentLevel);


        _uiActions.Enable();
        _uiActions.Escape.performed += OnEscapePressed;
        _uiActions.LevelSelect.performed += OnLevelSelectPressed;
    }

    private void OnDisable()
    {
        _uiActions.Disable();
        _uiActions.Escape.performed -= OnEscapePressed;
        _uiActions.LevelSelect.performed -= OnLevelSelectPressed;

        _levelSelectionUI.OnLevelPlayRequested -= LoadLevel;
        _levelSelectionUI.OnOpen -= PauseLevel;
        _levelSelectionUI.OnClose -= ResumeLevel;
        _pauseUI.OnOpen -= PauseLevel;
        _pauseUI.OnClose -= ResumeLevel;
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

        _levelSelectionUI.SetCurrentLevel(_currentLevel);
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

    public void PauseLevel()
    {
        if (!AreUIsOpen()) return;
        if (_levelObject != null) _levelObject.Pause(true);
    }

    public void ResumeLevel()
    {
        if (AreUIsOpen()) return;
        if (_levelObject != null) _levelObject.Pause(false);
    }

    public void ExitLevel()
    {
        UnloadLevel();
        ServiceLocator.Get<LoadingManger>().Load(SceneType.Menu);
    }

    private void OnApplicationFocus(bool focus)
    {
        if (!focus)
        {
            if (AreUIsOpen()) return;
            _pauseUI.Open();
        }
    }

    private bool AreUIsOpen()
    {
        return _levelSelectionUI.IsOpen || _pauseUI.IsOpen;
    }
    private void OnEscapePressed(InputAction.CallbackContext ctx)
    {
        if (_levelSelectionUI.IsOpen)
        {
            _levelSelectionUI.Close();
            return;
        }

        if (_pauseUI.IsOpen) _pauseUI.Close();
        else _pauseUI.Open();
    }

    private void OnLevelSelectPressed(InputAction.CallbackContext ctx)
    {
        if(_levelSelectionUI.IsOpen) _levelSelectionUI.Close();
        else _levelSelectionUI.Open();
    }

}
