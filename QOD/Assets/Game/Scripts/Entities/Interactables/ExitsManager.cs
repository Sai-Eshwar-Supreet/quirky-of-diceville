using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System.Threading;

public class ExitsManager : MonoBehaviour
{
    [SerializeField] private Exit[] _exits;
    [SerializeField] private TextMeshProUGUI _completionText;

    private void Awake()
    {
        _completionText.SetText($"0/{_exits.Length}");
    }

    private void OnEnable()
    {
        foreach (var exit in _exits)
        {
            exit.OnExitInteracted += HandleExitInteraction;
        }
    }

    private void OnDisable()
    {
        foreach (var exit in _exits)
        {
            exit.OnExitInteracted -= HandleExitInteraction;
        }
    }

    private async void HandleExitInteraction()
    {
        var count = 0;
        foreach (var exit in _exits)
        {
            if (exit.IsAppropriateMatch) count++;
        }

        _completionText.SetText($"{count}/{_exits.Length}");

        if (count != _exits.Length) return;
        
        await LevelManager.Instance.GoToNextLevel();
    }
}