using UnityEngine;

public class ExitsManager : MonoBehaviour
{
    [SerializeField] private Exit[] _exits;

    private void OnEnable()
    {
        foreach (var exit in _exits)
        {
            exit.OnExitInteracted += CheckAndExit;
        }
    }

    private void OnDisable()
    {
        foreach (var exit in _exits)
        {
            exit.OnExitInteracted -= CheckAndExit;
        }
    }

    private void CheckAndExit()
    {
        foreach (var exit in _exits)
        {
            if (!exit.IsAprropriateMatch) return;
        }

        GameManager.Instance.ExitLevel();
    }
}