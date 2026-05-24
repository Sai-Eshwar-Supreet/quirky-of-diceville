using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class that all Custom animation listners inherit from.
/// </summary>
public abstract class AnimationEventListener : MonoBehaviour
{
    protected void OnAnimate(List<CustomAnimation> animations)
    {
        if (animations == null || animations.Count == 0) return;
        foreach (var animation in animations) OnAnimate(animation);
    }
    protected void OnAnimate(CustomAnimation animation)
    {
        if (animation == null) return;
        animation.Animate(gameObject);
    }
}