using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// CustomEventHandler is a MonoBehaviour that implements multiple Unity event interfaces.
/// It provides a way to handle various UI events such as pointer clicks, pointer enter/exit, drag events, and selection events.
/// </summary>
public class CustomEventHandler : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, ISelectHandler, IDeselectHandler, IBeginDragHandler, IEndDragHandler
{
    /// <summary>
    /// Indicates whether the object is interactable. This is determined by the Selectable component if present.
    /// </summary>
    [SerializeField] private bool isInteractable;

    /// <summary>
    /// Event triggered when a pointer click occurs.
    /// </summary>
    public Action<PointerEventData> OnPointerClickState;

    /// <summary>
    /// Event triggered when a pointer down occurs.
    /// </summary>
    public Action<PointerEventData> OnPointerDownState;

    /// <summary>
    /// Event triggered when a pointer up occurs.
    /// </summary>
    public Action<PointerEventData> OnPointerUpState;

    /// <summary>
    /// Event triggered when the pointer enters the object.
    /// </summary>
    public Action<PointerEventData> OnPointerEntered;

    /// <summary>
    /// Event triggered when the pointer exits the object.
    /// </summary>
    public Action<PointerEventData> OnPointerExited;

    /// <summary>
    /// Event triggered when the object is selected.
    /// </summary>
    public Action<BaseEventData> OnSelected;

    /// <summary>
    /// Event triggered when the object is deselected.
    /// </summary>
    public Action<BaseEventData> OnDeselected;

    /// <summary>
    /// Event triggered when a drag begins.
    /// </summary>
    public Action<PointerEventData> OnDragBegin;

    /// <summary>
    /// Event triggered when a drag ends.
    /// </summary>
    public Action<PointerEventData> OnDragEnd;

    /// <summary>
    /// Reference to the Selectable component, if present.
    /// </summary>
    private Selectable _selectable;

    private bool _isPointerDown;
    private bool _hasPointerEntered;
    private bool _isSelected;
    private bool _isDragged;

    /// <summary>
    /// Gets whether the object is interactable. Updates the value based on the Selectable component if present.
    /// </summary>
    public bool IsInteractable
    {
        get
        {
            if (_selectable != null) isInteractable = _selectable.interactable;
            return isInteractable;
        }
    }
    private void Awake()
    {
        Initialize();
    }

    /// <summary>
    /// Initializes the component and checks for a Selectable component.
    /// </summary>
    public void Initialize()
    {
        if (gameObject.TryGetComponent(out _selectable)) isInteractable = _selectable.interactable;
    }

    /// <summary>
    /// Handles pointer click events.
    /// </summary>
    /// <param name="eventData">Pointer event data.</param>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (IsInteractable) OnPointerClickState?.Invoke(eventData);
    }

    /// <summary>
    /// Handles pointer down events.
    /// </summary>
    /// <param name="eventData">Pointer event data.</param>
    public void OnPointerDown(PointerEventData eventData)
    {
        if (IsInteractable)
        {
            _isPointerDown = true;
            OnPointerDownState?.Invoke(eventData);
        }
    }

    /// <summary>
    /// Handles pointer up events.
    /// </summary>
    /// <param name="eventData">Pointer event data.</param>
    public void OnPointerUp(PointerEventData eventData)
    {
        if (_isPointerDown)
        {
            _isPointerDown = false;
            OnPointerUpState?.Invoke(eventData);
        }
    }

    /// <summary>
    /// Handles pointer enter events.
    /// </summary>
    /// <param name="eventData">Pointer event data.</param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (IsInteractable)
        {
            _hasPointerEntered = true;
            OnPointerEntered?.Invoke(eventData);
        }
    }

    /// <summary>
    /// Handles pointer exit events.
    /// </summary>
    /// <param name="eventData">Pointer event data.</param>
    public void OnPointerExit(PointerEventData eventData)
    {
        if (_hasPointerEntered)
        {
            _hasPointerEntered = false;
            OnPointerExited?.Invoke(eventData);
        }
    }

    /// <summary>
    /// Handles selection events.
    /// </summary>
    /// <param name="eventData">Base event data.</param>
    public void OnSelect(BaseEventData eventData)
    {
        if (IsInteractable)
        {
            _isSelected = true;
            OnSelected?.Invoke(eventData);
        }
    }

    /// <summary>
    /// Handles deselection events.
    /// </summary>
    /// <param name="eventData">Base event data.</param>
    public void OnDeselect(BaseEventData eventData)
    {
        if (_isSelected)
        {
            _isSelected = false;
            OnDeselected?.Invoke(eventData);
        }
    }

    /// <summary>
    /// Handles the beginning of a drag event.
    /// </summary>
    /// <param name="eventData">Pointer event data.</param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (IsInteractable)
        {
            _isDragged = true;
            OnDragBegin?.Invoke(eventData);
        }
    }

    /// <summary>
    /// Handles the end of a drag event.
    /// </summary>
    /// <param name="eventData">Pointer event data.</param>
    public void OnEndDrag(PointerEventData eventData)
    {
        if (_isDragged)
        {
            _isDragged = false;
            OnDragEnd?.Invoke(eventData);
        }
    }
}