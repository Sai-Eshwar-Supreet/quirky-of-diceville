using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-100)]
public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] private GameDataManager _gameDataManager;
    [SerializeField] private SaveDataManager _saveDataManager;

    private void OnEnable()
    {
        _gameDataManager.Load();
        RegisterServices();
        _saveDataManager.Load();
    }

    private void OnDisable()
    {
        UnregisterServices();
    }

    private void RegisterServices()
    {
        ServiceLocator.Register(_gameDataManager);
        ServiceLocator.Register(_saveDataManager);
    }

    private void UnregisterServices()
    {
        ServiceLocator.Unregister<GameDataManager>();
        ServiceLocator.Unregister<SaveDataManager>();
    }

    public void ExitLevel()
    {

    }
}
