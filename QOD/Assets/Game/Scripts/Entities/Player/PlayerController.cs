using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerFSM[] _players;
    private readonly PlayerInputManager _playerInputManager = new();

    private int _currentPlayerIndex;


    protected void Awake()
    {
        if(_players.Length == 0) throw new System.Exception("No players assigned to PlayerController!");
        _playerInputManager.Init();
        _currentPlayerIndex = 0;
        _players[_currentPlayerIndex].SupplyActivationInput(true);
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

    private void OnSwitch()
    {
        _players[_currentPlayerIndex].SupplyActivationInput(false);
        _currentPlayerIndex = (_currentPlayerIndex + 1) % _players.Length;
        _players[_currentPlayerIndex].SupplyActivationInput(true);
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
