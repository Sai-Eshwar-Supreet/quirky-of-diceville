using System;
using System.Collections.Generic;
using UnityEngine;

public class ServiceLocator : MonoBehaviour
{
    private static readonly Dictionary<Type, object> _services = new();

    public static void Register<T>(T service)
    {
        var type = typeof(T);
        if (_services.ContainsKey(type))
        {
            Debug.LogWarning($"Service of type {type} is already registered.");
            return;
        }

        _services.Add(type, service);
    }

    public static void Unregister<T>()
    {
        var type = typeof(T);
        if (_services.ContainsKey(type)) { 
            _services.Remove(type);
            return;
        }
        
        Debug.LogWarning($"Service of type {type} is not registered.");
    }

    public static T Get<T>()
    {
        var type = typeof(T);
        if (_services.TryGetValue(type, out var service)) return (T)service;

        Debug.LogError($"Service of type {type} is not registered.");
        return default;
    }
}