using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneType
{
    Menu = 0,
    Level = 1,
}

public class LoadingManger : MonoBehaviour
{
    [SerializeField] private SceneLoader _loader;

    public event Action<SceneType> OnLoaded;

    public async void Load(SceneType type)
    {
        await _loader.LoadScene((int)type);
        OnLoaded?.Invoke(type);
    }
}
