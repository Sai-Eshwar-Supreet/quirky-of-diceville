using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-100)]
public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] private GameDataManager _gameDataManager;

    private void OnEnable()
    {
        _gameDataManager.Load();
        RegisterServices();
    }

    private void OnDisable()
    {
        UnregisterServices();
    }

    private void RegisterServices()
    {
        ServiceLocator.Register(_gameDataManager);
    }

    private void UnregisterServices()
    {
        ServiceLocator.Unregister<GameDataManager>();
    }

    public void ExitLevel()
    {

    }
}
