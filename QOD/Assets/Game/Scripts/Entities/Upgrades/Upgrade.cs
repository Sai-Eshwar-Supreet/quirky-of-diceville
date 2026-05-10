using UnityEngine;

public class Upgrade : MonoBehaviour
{
    [SerializeField] private DiceStateManager _diceStateManager;
    private void Awake()
    {
        _diceStateManager.Enable();
    }
    private void OnEnable()
    {
        var levelDataQueryService = ServiceLocator.Get<ILevelDataQueryService>();

        // when not of the given type, or when the upgrade is already unlocked, destroy the upgrade object
        var isInvalidColorUpgrade = !_diceStateManager.Type.HasFlag(DiceType.Color) || levelDataQueryService.IsColorUnlocked(_diceStateManager.ColorIndex);
        var isInvalidValueUpgrade = !_diceStateManager.Type.HasFlag(DiceType.Value) || levelDataQueryService.IsValueUnlocked(_diceStateManager.ValueIndex);
        var isInvalidUpgrade = isInvalidColorUpgrade && isInvalidValueUpgrade;
        if (isInvalidUpgrade) DestroyImmediate(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            var levelDataUpdateService = ServiceLocator.Get<ILevelDataUpdateService>();

            if (_diceStateManager.Type.HasFlag(DiceType.Color))
            {
                levelDataUpdateService.UnlockColor(_diceStateManager.ColorIndex);
            }
            
            if(_diceStateManager.Type.HasFlag(DiceType.Value))
            {
                levelDataUpdateService.UnlockValue(_diceStateManager.ValueIndex);
            }
            // Add visual or audio feedback here, such as playing a sound effect or showing a particle effect.
            Destroy(gameObject);
        }
    }

}
