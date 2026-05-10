using UnityEngine;
using Unity.Cinemachine;
using System;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerFSM[] _players;
    [SerializeField] private CinemachineCamera _camera;
    [SerializeField] private AppearanceChangeUI _appearanceChangeUI;
    private readonly PlayerInputManager _playerInputManager = new();

    private int _currentPlayerIndex;

    private void Awake()
    {
        _playerInputManager.Init();
        if(_players.Length == 0) throw new System.Exception("No players assigned to PlayerController!");
        _currentPlayerIndex = 0;
        _players[_currentPlayerIndex].SupplyActivationInput(true);
        _camera.Follow = _players[_currentPlayerIndex].transform;
    }

    private void OnEnable()
    {
        _playerInputManager.SetCursorState(true);
        var levelDataQueryService = ServiceLocator.Get<ILevelDataQueryService>();

        _playerInputManager.Enable();

        _playerInputManager.OnMove += OnMove;
        _playerInputManager.OnPlayerSwitch += OnSwitch;
        _playerInputManager.OnAppearanceMenuInput += OnAppearanceMenuUsed;
        
        levelDataQueryService.OnAppearanceUnlocked += _appearanceChangeUI.UpdateUnlocks;

        _appearanceChangeUI.OnColorSelected += OnColorSelected;
        _appearanceChangeUI.OnValueSelected += OnValueSelected;
    }

    private void OnDisable()
    {
        var levelDataQueryService = ServiceLocator.Get<ILevelDataQueryService>();
        
        _playerInputManager.OnMove -= OnMove;
        _playerInputManager.OnPlayerSwitch -= OnSwitch;
        _playerInputManager.OnAppearanceMenuInput -= OnAppearanceMenuUsed;


        if(levelDataQueryService != null) levelDataQueryService.OnAppearanceUnlocked -= _appearanceChangeUI.UpdateUnlocks;

        _appearanceChangeUI.OnColorSelected -= OnColorSelected;
        _appearanceChangeUI.OnValueSelected -= OnValueSelected;

        _playerInputManager.Disable();
        _playerInputManager.SetCursorState(false);
    }

    private void OnAppearanceMenuUsed(bool active)
    {
        if(active) { 
            _appearanceChangeUI.Open(
                _players[_currentPlayerIndex].ColorId, 
                _players[_currentPlayerIndex].ValueId
                ); 
            _playerInputManager.SetCursorState(false);
        }
        else {
            _appearanceChangeUI.Close();
            _playerInputManager.SetCursorState(true);
        }
    }

    private void OnValueSelected(int id)
    {
        _players[_currentPlayerIndex].SupplyValueId(id);
    }

    private void OnColorSelected(int id)
    {
        _players[_currentPlayerIndex].SupplyColorId(id);
    }

    private void OnSwitch(int direction)
    {
        if (_appearanceChangeUI.IsOpen) return;
        DeactivateCurrentPlayer();
        int length = _players.Length;
        _currentPlayerIndex = (length + _currentPlayerIndex + direction) % length;
        _players[_currentPlayerIndex].SupplyActivationInput(true);
        _camera.Follow = _players[_currentPlayerIndex].transform;
    }

    private void DeactivateCurrentPlayer()
    {
        var previousPlayer = _players[_currentPlayerIndex];
        previousPlayer.SupplyMoveInput(Vector2.zero);
        previousPlayer.SupplyActivationInput(false);
    }

    private void OnMove(Vector2 vector)
    {
        _players[_currentPlayerIndex].SupplyMoveInput(vector);
    }
}
