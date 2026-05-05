using DG.Tweening;
using System;
using UnityEngine;

public class PlayerFSM : BaseStateMachine
{
    [SerializeField] private Vector2 _moveOffset = Vector2.one;
    [SerializeField] private Ease _moveEase = Ease.InOutSine;
    [SerializeField] private float _hoverOffset = 1f;
    [SerializeField] private float _moveDuration = 0.5f;
    [SerializeField] private LayerMask _platformMask;

    public bool IsMovePressed => MoveInput != Vector2.zero;
    public Vector2 MoveInput { get; private set; }
    public bool IsHoverPressed { get; private set; } = false;

    public bool IsMoving { get; private set; } = false;

    public bool IsAcive { get; private set; } = false;

    public void SupplyMoveInput(Vector2 vector)
    {
        MoveInput = vector;
    }

    public void SupplyHoverInput(bool hoverPressed)
    {
        IsHoverPressed = hoverPressed;
    }

    public void SupplyActivationInput(bool isActive)
    {
        IsAcive = isActive;
    }

    public override void InitializeStates()
    {
        StateFactory.AddState(new IdleState(this));
        StateFactory.AddState(new MoveState(this));
        StateFactory.AddState(new HoverState(this));
        StateFactory.AddState(new InactiveState(this));

        CurrentState = StateFactory.GetState<IdleState>();
    }

    public void Move(Vector3 units)
    {
        if (IsMoving || units == Vector3.zero) return;
        var targetPos = transform.position + new Vector3(units.x * _moveOffset.x, units.y * _hoverOffset, units.z * _moveOffset.y);

        if (CheckGroundAvailability(targetPos) && CheckSpaceAvailability(targetPos))
        {
            IsMoving = true;
            transform.DOMove(targetPos, _moveDuration).SetEase(_moveEase).onComplete += () => IsMoving = false;
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
