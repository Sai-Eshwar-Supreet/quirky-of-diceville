using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ParticleDataRegistry", menuName = "Particle/DataRegistry")]
public class ParticleRegistry : ScriptableObject
{
    [SerializeField] private ParticleData[] _particleDataList;

    public IReadOnlyList<ParticleData> ParticlDataList => _particleDataList;
}
