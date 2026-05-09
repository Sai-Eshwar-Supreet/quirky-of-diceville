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
        var levelDataManager = ServiceLocator.Get<LevelDataManager>();

        // when not of the given type, or when the upgrade is already unlocked, destroy the upgrade object
        var isInvalidColorUpgrade = !_diceStateManager.Type.HasFlag(DiceType.Color) || levelDataManager.IsColorUnlocked(_diceStateManager.ColorIndex);
        var isInvalidValueUpgrade = !_diceStateManager.Type.HasFlag(DiceType.Value) || levelDataManager.IsValueUnlocked(_diceStateManager.ValueIndex);
        var isInvalidUpgrade = isInvalidColorUpgrade && isInvalidValueUpgrade;
        if (isInvalidUpgrade) DestroyImmediate(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            var levelDataManager = ServiceLocator.Get<LevelDataManager>();

            if (_diceStateManager.Type.HasFlag(DiceType.Color))
            {
                levelDataManager.UnlockColor(_diceStateManager.ColorIndex);
            }
            
            if(_diceStateManager.Type.HasFlag(DiceType.Value))
            {
                levelDataManager.UnlockValue(_diceStateManager.ValueIndex);
            }
            // Add visual or audio feedback here, such as playing a sound effect or showing a particle effect.
            Destroy(gameObject);
        }
    }

}
