using DG.Tweening;
using System;
using UnityEngine;

public class PlayerFSM : BaseStateMachine
{
    [SerializeField] private Vector2 _moveOffset = Vector2.one;
    [SerializeField] private float _moveDuration = 0.5f;
    private readonly PlayerInputManager _playerInputManager = new();

    public bool IsMovePressed => MoveInput != Vector2.zero;
    public Vector2 MoveInput { get; private set; }

    private bool _isMovePressed = false;

    protected override void Awake()
    {
        base.Awake();
        _playerInputManager.Init();
    }

    private void OnEnable()
    {
        _playerInputManager.Enable();

        _playerInputManager.OnMove += OnMove;
    }

    private void OnDisable()
    {
        _playerInputManager.OnMove -= OnMove;

        _playerInputManager.Disable();
    }

    private void OnMove(Vector2 vector)
    {
        MoveInput = vector;
    }

    public override void InitializeStates()
    {
        StateFactory.AddState(new IdleState(this));
        StateFactory.AddState(new MoveState(this));

        CurrentState = StateFactory.GetState<IdleState>();
    }

    public void Move(Vector3 units)
    {
        if (_isMovePressed) return;
        _isMovePressed = true;
        transform.DOMove(transform.position + new Vector3(units.x * _moveOffset.x, 0, units.z * _moveOffset.y), _moveDuration).onComplete += () => _isMovePressed = false;
    }
}
