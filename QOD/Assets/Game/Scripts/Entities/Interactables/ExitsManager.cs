using UnityEngine;
using System.Collections.Generic;

public class ExitsManager : MonoBehaviour
{
    [SerializeField] private Exit[] _exits;

    private void OnEnable()
    {
        foreach (var exit in _exits)
        {
            exit.OnExitInteracted += GoToNextLevel;
        }
    }

    private void OnDisable()
    {
        foreach (var exit in _exits)
        {
            exit.OnExitInteracted -= GoToNextLevel;
        }
    }

    private void GoToNextLevel()
    {
        foreach (var exit in _exits)
        {
            if (!exit.IsAprropriateMatch) return;
        }
        LevelManager.Instance.GoToNextLevel();
    }
}