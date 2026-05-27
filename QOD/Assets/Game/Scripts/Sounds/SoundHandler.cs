using UnityEngine;
using UnityEngine.EventSystems;
using static SoundManager;

/// <summary>
/// Handles sound effects for various events such as pointer clicks, pointer enter/exit, and drag events.
/// </summary>
public class SoundHandler : MonoBehaviour
{
    /// <summary>
    /// Reference to the CustomEventHandler that triggers events.
    /// </summary>
    [SerializeField] protected CustomEventHandler _eventHandler;

    /// <summary>
    /// Audio config played when a pointer click event occurs.
    /// </summary>
    [SerializeField] private SoundConfig _clickConfig;


    /// <summary>
    /// Audio config played when a pointer enters the object.
    /// </summary>
    [SerializeField] private SoundConfig _pointerEnterConfig;

    /// <summary>
    /// Audio config played when a pointer exits the object.
    /// </summary>
    [SerializeField] private SoundConfig _pointerExitConfig;


    /// <summary>
    /// Audio config played when a drag begins.
    /// </summary>
    [SerializeField] private SoundConfig _beginDragConfig;

    /// <summary>
    /// Audio config played when a drag ends.
    /// </summary>
    [SerializeField] private SoundConfig _endDragConfig;

    protected virtual void Awake()
    {
        RegisterEvents();
    }
    protected virtual void OnDestroy()
    {
        UnregisterEvents();
    }

    /// <summary>
    /// Subscribes to UI event handlers.
    /// </summary>
    public void RegisterEvents()
    {
        if (_eventHandler != null)
        {
            _eventHandler.OnPointerClickState += OnPointerClick;
            _eventHandler.OnPointerEntered += OnPointerEnter;
            _eventHandler.OnPointerExited += OnPointerExit;
            _eventHandler.OnDragBegin += OnBeginDrag;
            _eventHandler.OnDragEnd += OnEndDrag;
        }
    }

    /// <summary>
    /// Unsubscribes from UI event handlers.
    /// </summary>
    public void UnregisterEvents()
    {
        if (_eventHandler != null)
        {
            _eventHandler.OnPointerClickState -= OnPointerClick;
            _eventHandler.OnPointerEntered -= OnPointerEnter;
            _eventHandler.OnPointerExited -= OnPointerExit;
            _eventHandler.OnDragBegin -= OnBeginDrag;
            _eventHandler.OnDragEnd -= OnEndDrag;
        }
    }

    /// <summary>
    /// Plays the sound effect for the begin drag event.
    /// </summary>
    /// <param name="eventData">Pointer event data.</param>
    protected virtual void OnBeginDrag(PointerEventData eventData)
    {
        if (_beginDragConfig != null) Play(_beginDragConfig);
    }

    /// <summary>
    /// Plays the sound effect for the end drag event.
    /// </summary>
    /// <param name="eventData">Pointer event data.</param>
    protected virtual void OnEndDrag(PointerEventData eventData)
    {
        if (_endDragConfig != null) Play(_endDragConfig);
    }

    /// <summary>
    /// Plays the sound effect for the pointer click event.
    /// </summary>
    /// <param name="eventData">Pointer event data.</param>
    protected virtual void OnPointerClick(PointerEventData eventData)
    {
        if (_clickConfig != null) Play(_clickConfig);
    }

    /// <summary>
    /// Plays the sound effect for the pointer enter event.
    /// </summary>
    /// <param name="eventData">Pointer event data.</param>
    protected virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (_pointerEnterConfig != null) Play(_pointerEnterConfig);
    }

    /// <summary>
    /// Plays the sound effect for the pointer exit event.
    /// </summary>
    /// <param name="eventData">Pointer event data.</param>
    protected virtual void OnPointerExit(PointerEventData eventData)
    {
        if (_pointerExitConfig != null) Play(_pointerExitConfig);
    }
}