using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Collider))]
public class Key : MonoBehaviour
{
    private enum KeyType
    {
        OneTime,
        PressurePlate
    }

    [SerializeField] private List<Door> _doors;
    [SerializeField] private KeyType _keyType;
    [SerializeField] private DiceStateManager _diceStateManager;
    [SerializeField] private Collider _triggerCollider;

    [Header("Sounds")]
    [SerializeField] private SoundConfig _releasedSound;
    [SerializeField] private SoundConfig _pressedSound;

    public bool IsPressed { get; private set; } = false;

    private void Awake()
    {
        _diceStateManager.Enable();
    }

    private void OnTriggerEnter(Collider other) => Interact(other);
    private void OnTriggerStay(Collider other) => Interact(other);
    private void OnTriggerExit(Collider other) => ExitInteraction(other);

    private void Interact(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var playerDiceState = other.gameObject.GetComponent<DiceStateManager>();

            var previousState = IsPressed;
            IsPressed = _diceStateManager.Matches(playerDiceState);

            if (previousState == IsPressed) return;

            SoundManager.Play(IsPressed ? _pressedSound : _releasedSound, $"Key state sound : {gameObject.GetEntityId()}");


            if (IsPressed && _keyType == KeyType.OneTime)
            {
                _diceStateManager.Disable();
                _triggerCollider.enabled = false;
            }

            UpdateDoorStates();
        }
    }

    private void ExitInteraction(Collider other)
    {
        if (other.CompareTag("Player") && _keyType == KeyType.PressurePlate)
        {
            if(IsPressed) SoundManager.Play(_releasedSound, $"Key state sound : {gameObject.GetEntityId()}"); // if pressed, release it

            IsPressed = false;
            UpdateDoorStates();
        }
    }

    private void UpdateDoorStates()
    {
        foreach (var door in _doors)
        {
            door.SetLockState(IsPressed);
        }
    }
}
