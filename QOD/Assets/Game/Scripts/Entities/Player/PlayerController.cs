using UnityEngine;
using Unity.Cinemachine;
using System;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerFSM[] _players;
    [SerializeField] private int _startPlayerIndex = 0;
    [SerializeField] private PlayerUI _playerUI;
    [SerializeField] private CameraController _cameraController;
    [SerializeField] private AppearanceChangeUI _appearanceChangeUI;

    [Header("Sounds")]
    [SerializeField] private SoundConfig _playerSwitchSound;


    private readonly PlayerInputManager _playerInputManager = new();

    private int _currentPlayerIndex;

    private void Awake()
    {
        _playerInputManager.Init();
        if(_players.Length == 0) throw new System.Exception("No players assigned to PlayerController!");

        _currentPlayerIndex = GetWrappedPlayerIndex(_startPlayerIndex);
        _players[_currentPlayerIndex].SupplyActivationInput(true);

        _cameraController.MoveImmediateTo(_players[_currentPlayerIndex].transform);

        _playerUI.AddPlayers(_players.Length);

        _playerUI.SwitchTo(_currentPlayerIndex);
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

        OnMove(Vector2.zero);
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

        var newPlayerIndex = GetWrappedPlayerIndex(_currentPlayerIndex + direction);

        if (newPlayerIndex == _currentPlayerIndex) return;

        SoundManager.Play(_playerSwitchSound, "Player Switch");

        DeactivateCurrentPlayer();
        _currentPlayerIndex = newPlayerIndex;
        _players[_currentPlayerIndex].SupplyActivationInput(true);

        _cameraController.MoveTo(_players[_currentPlayerIndex].transform);

        _playerUI.SwitchTo(_currentPlayerIndex);
    }

    private int GetWrappedPlayerIndex(int index)
    {
        int length = _players.Length;
        var wrappedIndex = (length + index) % length;
        return wrappedIndex;
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
