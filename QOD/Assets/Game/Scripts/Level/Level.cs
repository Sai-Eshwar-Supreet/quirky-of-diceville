using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private PlayerController _playerController;

    public bool IsPaused => _playerController == null || !_playerController.enabled;

    public void Pause(bool paused)
    {
        if (paused == IsPaused) return;
        _playerController.enabled = !paused;
    }
}
