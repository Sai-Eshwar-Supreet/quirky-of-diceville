using UnityEngine;

public abstract class BaseSaveService : ScriptableObject
{
    public abstract string BaseURL { get; }

    public abstract T Load<T>(string route);
    public abstract void Save<T>(string route, T data);
}
