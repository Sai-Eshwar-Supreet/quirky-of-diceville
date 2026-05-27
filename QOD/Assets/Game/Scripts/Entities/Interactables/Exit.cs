using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Exit : MonoBehaviour
{
    [SerializeField] private DiceStateManager _diceStateManager;
    [SerializeField] private Collider _triggerCollider;

    [Header("Effects")]
    [SerializeField] private ParticleSystem _validPS;
    [SerializeField] private ParticleSystem _invalidPS;

    [Header("Sounds")]
    [SerializeField] private SoundConfig _validSound;
    [SerializeField] private SoundConfig _invalidSound;

    public bool IsAppropriateMatch { get; private set; } = false;

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

            var previousMatchState = IsAppropriateMatch;
            IsAppropriateMatch = _diceStateManager.Matches(playerDiceState);

            if(previousMatchState !=  IsAppropriateMatch)
            {
                SoundManager.Play(IsAppropriateMatch? _validSound : _invalidSound , "Exit sound");
                UpdateParticleSystem(IsAppropriateMatch);
            }


            OnExitInteracted?.Invoke();
        }
    }
    private void ExitInteraction(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(IsAppropriateMatch) // exiting an appropriate match should trigger invalid sound and appropriate ps
            {
                SoundManager.Play(_invalidSound, "Exit sound");
                UpdateParticleSystem(false);
            }
            IsAppropriateMatch = false;

            OnExitInteracted?.Invoke();
        }
    }

    private void UpdateParticleSystem(bool valid)
    {
        if (valid)
        {
            _invalidPS.Clear();
            _validPS.Emit(1);
        }
        else
        {
            _validPS.Clear();
            _invalidPS.Emit(1);
        }
    }
}
