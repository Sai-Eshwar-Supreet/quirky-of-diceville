using DG.Tweening;
using UnityEngine;
using static Unity.VisualScripting.Member;
using static UnityEngine.Rendering.STP;

public class PlayerFSM : BaseStateMachine
{
    [SerializeField] private Vector2 _moveOffset = Vector2.one;
    [SerializeField] private Ease _moveEase = Ease.InOutSine;
    [SerializeField] private float _moveDuration = 0.5f;
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private LayerMask _obstacleMask;
    [SerializeField] private DiceStateManager _diceStateManager;
    [SerializeField] private PlayerAnimator _playerAnimator;

    [Header("Sounds")]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private SoundConfig _moveSound;
    public bool IsMovePressed => MoveInput != Vector2.zero;
    public Vector2 MoveInput { get; private set; }

    public bool IsMoving { get; private set; } = false;

    public bool IsAcive { get; private set; } = false;
    public int ColorId => _diceStateManager.ColorIndex;
    public int ValueId => _diceStateManager.ValueIndex;

    private Tween _moveTween;
    public PlayerAnimator PlayerAnimator => _playerAnimator;

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

        var targetPos = transform.position + GetMove();

        if (CheckGroundAvailability(targetPos) && CheckSpaceAvailability(targetPos))
        {
            SetIsMoving(true);
            PlayerAnimator.SetMove(MoveInput);
            SoundManager.PlayDedicated(_moveSound, _audioSource);

            _moveTween = transform.DOMove(targetPos, _moveDuration).SetEase(_moveEase);
            _moveTween.onComplete += OnMoveComplete;
        }
    }

    private void OnMoveComplete()
    {
        PlayerAnimator.SetMove(MoveInput);
        SetIsMoving(false);
    }

    private void SetIsMoving(bool isMoving)
    {
        IsMoving = isMoving;
        PlayerAnimator.SetIsMoving(IsMoving);
    }

    private Vector3 GetMove()
    {
        var forward = transform.forward;
        var right = transform.right;
        forward.y = right.y = 0;

        forward.Normalize();
        right.Normalize();

        var move = _moveOffset.x * MoveInput.x * right + _moveOffset.y * MoveInput.y * forward;

        return move;
    }

    private bool CheckGroundAvailability(Vector3 requestedLocation, float maxDistance = 0.6f)
    {
        Vector3 direction = Vector3.down;
        var ray = new Ray(requestedLocation, direction);

        bool isGroundAvailable = Physics.Raycast(ray, maxDistance, _groundMask, QueryTriggerInteraction.Ignore);
        return isGroundAvailable;
    }
    private bool CheckSpaceAvailability(Vector3 requestedLocation, float maxDistance = 1f)
    {
        Vector3 direction = (requestedLocation - transform.position).normalized;

        var ray = new Ray(transform.position, direction);
        bool isSpaceAvailable = !Physics.Raycast(ray, maxDistance, _obstacleMask, QueryTriggerInteraction.Ignore);
        return isSpaceAvailable;
    }
}
