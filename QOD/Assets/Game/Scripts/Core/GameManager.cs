using UnityEngine;

[DefaultExecutionOrder(-100)]
public class GameManager : MonoBehaviour
{
    [SerializeField] private GameDataManager _gameDataManager;
    [SerializeField] private LoadingManger _loadingManger;

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
        ServiceLocator.Register(_loadingManger);
    }

    private void UnregisterServices()
    {
        ServiceLocator.Unregister<GameDataManager>();
        ServiceLocator.Unregister<LoadingManger>();
    }
}
