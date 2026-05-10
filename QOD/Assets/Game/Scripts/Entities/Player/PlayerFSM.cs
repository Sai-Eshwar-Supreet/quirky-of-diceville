using DG.Tweening;
using UnityEngine;

public class PlayerFSM : BaseStateMachine
{
    [SerializeField] private Vector2 _moveOffset = Vector2.one;
    [SerializeField] private Ease _moveEase = Ease.InOutSine;
    [SerializeField] private float _moveDuration = 0.5f;
    [SerializeField] private LayerMask _platformMask;
    [SerializeField] private DiceStateManager _diceStateManager;

    public bool IsMovePressed => MoveInput != Vector2.zero;
    public Vector2 MoveInput { get; private set; }

    public bool IsMoving { get; private set; } = false;

    public bool IsAcive { get; private set; } = false;
    public int ColorId => _diceStateManager.ColorIndex;
    public int ValueId => _diceStateManager.ValueIndex;

    private Tween _moveTween;

    protected override void Awake()
    {
        base.Awake();

        _diceStateManager.Enable();
    }

    private void OnDisable()
    {
        if (_moveTween.IsActive()) _moveTween?.Kill();
    }

    public void SupplyMoveInput(Vector2 vector)
    {
        MoveInput = vector;
    }

    public void SupplyActivationInput(bool isActive)
    {
        IsAcive = isActive;
    }

    public void SupplyColorId(int id)
    {
        _diceStateManager.SetColor(id);
    }

    public void SupplyValueId(int id)
    {
        _diceStateManager.SetValue(id);
    }

    public override void InitializeStates()
    {
        StateFactory.AddState(new IdleState(this));
        StateFactory.AddState(new MoveState(this));
        StateFactory.AddState(new InactiveState(this));

        CurrentState = StateFactory.GetState<IdleState>();
    }

    public void Move()
    {
        if (IsMoving || !IsMovePressed) return;
        var targetPos = transform.position + new Vector3(MoveInput.x * _moveOffset.x, 0, MoveInput.y * _moveOffset.y);

        if (CheckGroundAvailability(targetPos) && CheckSpaceAvailability(targetPos))
        {
            IsMoving = true;
            _moveTween = transform.DOMove(targetPos, _moveDuration).SetEase(_moveEase);
            _moveTween.onComplete += () => IsMoving = false;
        }
    }

    private bool CheckGroundAvailability(Vector3 requestedLocation, float maxDistance = 1f)
    {
        Vector3 direction = Vector3.down;
        var ray = new Ray(requestedLocation, direction);
        bool isGroundAvailable = Physics.Raycast(ray, maxDistance, _platformMask, QueryTriggerInteraction.Ignore);
        return isGroundAvailable;
    }
    private bool CheckSpaceAvailability(Vector3 requestedLocation, float maxDistance = 1f)
    {
        Vector3 direction = (requestedLocation - transform.position).normalized;
        var ray = new Ray(transform.position, direction);
        bool isSpaceAvailable = !Physics.Raycast(ray, maxDistance, _platformMask, QueryTriggerInteraction.Ignore);
        return isSpaceAvailable;
    }
}
