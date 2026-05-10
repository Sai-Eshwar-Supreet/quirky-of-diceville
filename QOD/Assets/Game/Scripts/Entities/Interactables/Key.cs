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

            IsPressed = _diceStateManager.Matches(playerDiceState);

            if (IsPressed && _keyType == KeyType.OneTime)
            {
                _diceStateManager.Disable();
                _triggerCollider.enabled = false;
            }

            foreach (var door in _doors)
            {
                door.Unlock();
            }
        }
    }

    private void ExitInteraction(Collider other)
    {
        if (other.CompareTag("Player") && _keyType == KeyType.PressurePlate)
        {
            IsPressed = false;
            foreach (var door in _doors)
            {
                door.Lock();
            }
        }
    }
}
