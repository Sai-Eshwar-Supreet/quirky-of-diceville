using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-5)]
[DisallowMultipleComponent]
public class DDOLMarker : MonoBehaviour
{
    public static readonly HashSet<string> ExistingDDOLObjects = new();

    [SerializeField] private bool _useMarker = true;
    [SerializeField] private string _key = string.Empty;
    [SerializeField] private GameObject _childrenContainer;
    private void Awake()
    {
        if (!_useMarker)
            return;


        if (ExistingDDOLObjects.Contains(_key))
        {
            Destroy(gameObject);
            return;
        }

        _childrenContainer.SetActive(true);
        DontDestroyOnLoad(gameObject);
        ExistingDDOLObjects.Add(_key);
    }
}