using System;
using UnityEngine;

public class Exit : MonoBehaviour
{
    [SerializeField] private DiceStateManager _diceStateManager;
    [SerializeField] private Collider _triggerCollider;
    public bool IsAprropriateMatch { get; private set; } = false;

    public event Action OnExitInteracted;
    private void Awake()
    {
        _diceStateManager.Init();
    }

    private void OnTriggerEnter(Collider other)
    {
        Interact(other);
    }

    private void OnTriggerStay(Collider other)
    {
        Interact(other);
    }

    private void OnTriggerExit(Collider other)
    {
        ExitInteraction(other);
    }


    private void Interact(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var playerDiceState = other.gameObject.GetComponent<DiceStateManager>();
            var playerColor = playerDiceState.ColorIndex;
            var playerValue = playerDiceState.ValueIndex;

            IsAprropriateMatch = (playerColor == _diceStateManager.ColorIndex && playerValue == _diceStateManager.ValueIndex);
            Debug.Log($"{other.gameObject}  | {gameObject} | {IsAprropriateMatch} => {playerColor} == {_diceStateManager.ColorIndex} && {playerValue} == {_diceStateManager.ValueIndex}");
            OnExitInteracted?.Invoke();
        }
    }
    private void ExitInteraction(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IsAprropriateMatch = false;
            OnExitInteracted?.Invoke();
        }
    }
}
