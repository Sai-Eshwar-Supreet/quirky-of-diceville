using System.Threading.Tasks;
using UnityEngine;

public abstract class BaseSaveService : ScriptableObject
{
    public abstract string BaseURL { get; }

    public abstract Task<T> Load<T>(string route);
    public abstract Task Save<T>(string route, T data);
}
