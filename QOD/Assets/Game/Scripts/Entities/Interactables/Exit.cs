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
        _diceStateManager.Enable();
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

            IsAprropriateMatch = _diceStateManager.Matches(playerDiceState);
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
