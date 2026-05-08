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
        _diceStateManager.Init();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            var playerDiceState = other.gameObject.GetComponent<DiceStateManager>();
            var playerColor = playerDiceState.ColorIndex;
            var playerValue = playerDiceState.ValueIndex;

            IsPressed = (playerColor == _diceStateManager.ColorIndex) && (playerValue == _diceStateManager.ValueIndex);

            if( IsPressed && _keyType == KeyType.OneTime)
            {
                _diceStateManager.ResetDice();
                _triggerCollider.enabled = false;
            }

            foreach(var door in _doors)
            {
                door.Unlock();
            }
        }
    }

    private void OnTriggerExit(Collider other)
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
