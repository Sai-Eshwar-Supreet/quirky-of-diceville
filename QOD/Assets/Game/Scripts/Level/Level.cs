using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private PlayerController _playerController;

    public void Pause(bool paused)
    {
        _playerController.enabled = !paused;
    }
}
