using UnityEngine;
using Unity.Cinemachine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerFSM[] _players;
    [SerializeField] private CinemachineCamera _camera;
    private readonly PlayerInputManager _playerInputManager = new();

    private int _currentPlayerIndex;

    protected void Awake()
    {
        if(_players.Length == 0) throw new System.Exception("No players assigned to PlayerController!");
        _playerInputManager.Init();
        _currentPlayerIndex = 0;
        _players[_currentPlayerIndex].SupplyActivationInput(true);
        _camera.Follow = _players[_currentPlayerIndex].transform;
    }

    private void OnEnable()
    {
        _playerInputManager.Enable();

        _playerInputManager.OnMove += OnMove;
        _playerInputManager.OnHover += OnHover;
        _playerInputManager.OnPlayerSwitch += OnSwitch;
    }

    private void OnDisable()
    {
        _playerInputManager.OnMove -= OnMove;
        _playerInputManager.OnHover -= OnHover;
        _playerInputManager.OnPlayerSwitch -= OnSwitch;

        _playerInputManager.Disable();
    }

    private void OnSwitch(int direction)
    {
        ResetCurrentPlayer();
        int length = _players.Length;
        _currentPlayerIndex = (length + _currentPlayerIndex + direction) % length;
        _players[_currentPlayerIndex].SupplyActivationInput(true);
        _camera.Follow = _players[_currentPlayerIndex].transform;
    }

    private void ResetCurrentPlayer()
    {
        var previousPlayer = _players[_currentPlayerIndex];
        previousPlayer.SupplyMoveInput(Vector2.zero);
        previousPlayer.SupplyHoverInput(false);
        previousPlayer.SupplyActivationInput(false);
    }

    private void OnMove(Vector2 vector)
    {
        _players[_currentPlayerIndex].SupplyMoveInput(vector);
    }

    private void OnHover(bool hoverPressed)
    {
        _players[_currentPlayerIndex].SupplyHoverInput(hoverPressed);
    }
}
