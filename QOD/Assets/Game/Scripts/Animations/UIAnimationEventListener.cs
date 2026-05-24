using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIAnimationEventListener : AnimationEventListener
{
    [SerializeField] private CustomEventHandler animationHandler;
    [SerializeField] private List<CustomAnimation> pointerEnterAnimations;
    [SerializeField] private List<CustomAnimation> pointerExitAnimations;
    [SerializeField] private List<CustomAnimation> pointerDownAnimations;
    [SerializeField] private List<CustomAnimation> pointerUpAnimations;
    [SerializeField] private List<CustomAnimation> selectAnimations;
    [SerializeField] private List<CustomAnimation> deselectAnimations;
    [SerializeField] private List<CustomAnimation> enableAnimations;
    [SerializeField] private List<CustomAnimation> disableAnimations;
    private void OnEnable()
    {
        if (animationHandler != null)
        {
            animationHandler.OnPointerDownState += OnPointerDown;
            animationHandler.OnPointerUpState += OnPointerUp;
            animationHandler.OnPointerEntered += OnPointerEnter;
            animationHandler.OnPointerExited += OnPointerExit;
            animationHandler.OnSelected += OnSelect;
            animationHandler.OnDeselected += OnDeselect;
        }
        OnEnabled();
    }
    private void OnDisable()
    {
        if (animationHandler != null)
        {
            OnAnimate(pointerExitAnimations);
            OnAnimate(pointerUpAnimations);
            animationHandler.OnPointerDownState -= OnPointerDown;
            animationHandler.OnPointerUpState -= OnPointerUp;
            animationHandler.OnPointerEntered -= OnPointerEnter;
            animationHandler.OnPointerExited -= OnPointerExit;
        }
        OnDisabled();
    }
    private void OnPointerDown(PointerEventData eventData) => OnAnimate(pointerDownAnimations);
    private void OnPointerUp(PointerEventData eventData) => OnAnimate(pointerUpAnimations);
    private void OnPointerEnter(PointerEventData eventData) => OnAnimate(pointerEnterAnimations);
    private void OnPointerExit(PointerEventData eventData) => OnAnimate(pointerExitAnimations);
    private void OnSelect(BaseEventData eventData) => OnAnimate(selectAnimations);
    private void OnDeselect(BaseEventData eventData) => OnAnimate(deselectAnimations);
    private void OnEnabled() => OnAnimate(enableAnimations);
    private void OnDisabled() => OnAnimate(disableAnimations);

}