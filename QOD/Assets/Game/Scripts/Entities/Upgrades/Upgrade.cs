using UnityEngine;

public class Upgrade : MonoBehaviour
{
    [SerializeField] private DiceStateManager _diceStateManager;
    private void Awake()
    {
        _diceStateManager.Init();
    }
    private void OnEnable()
    {
        var gameDataManager = ServiceLocator.Get<GameDataManager>();

        Debug.LogWarning($" {gameObject.name}: {_diceStateManager.ColorIndex} = {gameDataManager.IsColorUnlocked(_diceStateManager.ColorIndex)} && {_diceStateManager.ValueIndex} = {gameDataManager.IsValueUnlocked(_diceStateManager.ValueIndex)}");

        // when not of the given type, or when the upgrade is already unlocked, destroy the upgrade object
        var isInvalidColorUpgrade = !_diceStateManager.Type.HasFlag(DiceType.Color) || gameDataManager.IsColorUnlocked(_diceStateManager.ColorIndex);
        var isInvalidValueUpgrade = !_diceStateManager.Type.HasFlag(DiceType.Value) || gameDataManager.IsValueUnlocked(_diceStateManager.ValueIndex);
        var isInvalidUpgrade = isInvalidColorUpgrade && isInvalidValueUpgrade;
        if (isInvalidUpgrade) DestroyImmediate(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            var gameDataManager = ServiceLocator.Get<GameDataManager>();

            if (_diceStateManager.Type.HasFlag(DiceType.Color))
            {
                gameDataManager.UnlockColor(_diceStateManager.ColorIndex);
            }
            
            if(_diceStateManager.Type.HasFlag(DiceType.Value))
            {
                gameDataManager.UnlockValue(_diceStateManager.ValueIndex);
            }
            // Add visual or audio feedback here, such as playing a sound effect or showing a particle effect.
            Destroy(gameObject);
        }
    }

}
