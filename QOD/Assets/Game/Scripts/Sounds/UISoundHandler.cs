using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>  
/// Handles sound effects for UI interactions such as pointer clicks, pointer enter/exit, and drag events.  
/// </summary>  
public class UISoundHandler : SoundHandler
{
    /// <summary>  
    /// Reference to the Selectable component associated with the UI element.  
    /// </summary>  
    private Selectable _selectable;

    protected override void Awake()
    {
        base.Awake();
        _selectable = _eventHandler.GetComponent<Selectable>();
    }

    /// <summary>  
    /// Plays a sound effect when a pointer click event occurs, if the associated UI element is interactable.  
    /// </summary>  
    /// <param name="eventData">Pointer event data containing information about the click event.</param>  
    protected override void OnPointerClick(PointerEventData eventData)
    {
        if (_selectable == null) return;
        if (_selectable.interactable) base.OnPointerClick(eventData);
    }

    /// <summary>  
    /// Plays a sound effect when a pointer enters the UI element, if the element is interactable.  
    /// </summary>  
    /// <param name="eventData">Pointer event data containing information about the pointer enter event.</param>  
    protected override void OnPointerEnter(PointerEventData eventData)
    {
        if (_selectable == null) return;
        if (_selectable.interactable) base.OnPointerEnter(eventData);
    }

    /// <summary>  
    /// Plays a sound effect when a pointer exits the UI element, if the element is interactable.  
    /// </summary>  
    /// <param name="eventData">Pointer event data containing information about the pointer exit event.</param>  
    protected override void OnPointerExit(PointerEventData eventData)
    {
        if (_selectable == null) return;
        if (_selectable.interactable) base.OnPointerExit(eventData);
    }

    /// <summary>  
    /// Plays a sound effect when a drag begins on the UI element, if the element is interactable.  
    /// </summary>  
    /// <param name="eventData">Pointer event data containing information about the drag begin event.</param>  
    protected override void OnBeginDrag(PointerEventData eventData)
    {
        if (_selectable == null) return;
        if (_selectable.interactable) base.OnBeginDrag(eventData);
    }

    /// <summary>  
    /// Plays a sound effect when a drag ends on the UI element, if the element is interactable.  
    /// </summary>  
    /// <param name="eventData">Pointer event data containing information about the drag end event.</param>  
    protected override void OnEndDrag(PointerEventData eventData)
    {
        if (_selectable == null) return;
        if (_selectable.interactable) base.OnEndDrag(eventData);
    }
}