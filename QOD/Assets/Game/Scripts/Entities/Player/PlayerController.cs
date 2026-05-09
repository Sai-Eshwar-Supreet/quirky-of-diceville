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

    protected void Awake()
    {
        if(_players.Length == 0) throw new System.Exception("No players assigned to PlayerController!");
        _playerInputManager.Init();
        _currentPlayerIndex = 0;
        _players[_currentPlayerIndex].SupplyActivationInput(true);
        _camera.Follow = _players[_currentPlayerIndex].transform;

        _appearanceChangeUI.Init();
    }

    private void OnEnable()
    {
        _playerInputManager.SetCursorState(true);
        var levelDataManager = ServiceLocator.Get<LevelDataManager>();

        _playerInputManager.Enable();

        _playerInputManager.OnMove += OnMove;
        _playerInputManager.OnHover += OnHover;
        _playerInputManager.OnPlayerSwitch += OnSwitch;
        _playerInputManager.OnAppearanceMenuInput += OnAppearanceMenuUsed;
        
        levelDataManager.OnAppearanceUnlocked += _appearanceChangeUI.UpdateUnlocks;

        _appearanceChangeUI.OnColorSelected += OnColorSelected;
        _appearanceChangeUI.OnValueSelected += OnValueSelected;
    }

    private void OnDisable()
    {
        var levelDataManager = ServiceLocator.Get<LevelDataManager>();
        
        _playerInputManager.OnMove -= OnMove;
        _playerInputManager.OnHover -= OnHover;
        _playerInputManager.OnPlayerSwitch -= OnSwitch;
        _playerInputManager.OnAppearanceMenuInput -= OnAppearanceMenuUsed;


        if(levelDataManager != null) levelDataManager.OnAppearanceUnlocked -= _appearanceChangeUI.UpdateUnlocks;

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
