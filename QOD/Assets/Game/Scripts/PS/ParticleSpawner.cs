using System.Collections.Generic;
using UnityEngine;

public class ParticleSpawner : MonoSingleton<ParticleSpawner>
{
    [SerializeField] private ParticleRegistry _particleDataRegistry;

    private readonly Dictionary<string, ParticleData> _particleCache = new();

    protected override void Awake()
    {
        base.Awake();

        _particleCache.Clear();

        foreach(var particleData in _particleDataRegistry.ParticlDataList)
        {
            if(!_particleCache.TryAdd(particleData.ID, particleData))
            {
                Debug.LogError($"Duplicate particle ID: {particleData.ID}");
            }
        }
    }

    public void SpawnAt(string id, Vector3 worldPosition)
    {
        if( _particleCache.TryGetValue(id, out var particleData))
        {
            Instantiate(particleData.ParticlePrefab, worldPosition, Quaternion.identity);
        }
    }
}
