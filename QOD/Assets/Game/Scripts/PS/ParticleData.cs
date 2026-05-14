using UnityEngine;

[CreateAssetMenu(fileName = "ParticleData", menuName = "Particle/Data")
    ]
public class ParticleData : ScriptableObject
{
    [SerializeField] private string _id;
    [SerializeField] private GameObject _particlePrefab;

    public string ID => _id;
    public GameObject ParticlePrefab => _particlePrefab;
}
