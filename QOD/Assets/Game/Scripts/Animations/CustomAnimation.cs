using DG.Tweening;
using UnityEngine;

public abstract class CustomAnimation : ScriptableObject
{
    [SerializeField] protected float _time;
    [SerializeField] protected Ease _ease;

    /// <summary>
    /// Animates the target based on the specific implementation.
    /// </summary>
    /// <param name="target">The target object to animate</param>
    public abstract void Animate(GameObject target);
}