using System;
using System.Collections.Generic;

public class StateFactory
{
    private readonly Dictionary<Type, IState> _statesCache;

    public StateFactory()
    {
        _statesCache = new Dictionary<Type, IState>();
    }
    /// <summary>
    /// Add a new state
    /// </summary>
    /// <typeparam name="T">Type of the state. Should be a child class of IState.</typeparam>
    /// <param name="state">Instance of the state</param>
    /// <returns>Returns true, if the state is added successfully.</returns>
    public bool AddState<T>(T state) where T : IState
    {
        Type key = typeof(T);
        if (_statesCache.ContainsKey(key)) return false;
        else
        {
            _statesCache.Add(key, state);
            return true;
        }
    }
    /// <summary>
    /// Remove an existing state
    /// </summary>
    /// <typeparam name="T">Type of the state. Should be a child class of IState.</typeparam>
    /// <returns>Returns true, if the state is removed successfully.</returns>
    public bool RemoveState<T>() where T : IState
    {
        Type key = typeof(T);
        if (_statesCache.ContainsKey(key)) return _statesCache.Remove(key);
        return false;
    }
    /// <summary>
    /// Get the cached state.
    /// </summary>
    /// <typeparam name="T">Type of the state. Should be a child class of IState.</typeparam>
    /// <returns>Returns the instance of the cached state.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the state is not available in the cache.</exception>
    public T GetState<T>() where T : IState
    {
        Type key = typeof(T);
        if (_statesCache.TryGetValue(key, out IState state)) return (T)state;
        else throw new InvalidOperationException($"No state associated with key - {key}");
    }
    /// <summary>
    /// Clear all the cached states.
    /// </summary>
    public void ClearStates()
    {
        _statesCache.Clear();
    }

}