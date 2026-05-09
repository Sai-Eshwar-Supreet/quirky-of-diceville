using UnityEngine;

public class MoveState : BaseState<PlayerFSM>
{
    public MoveState(PlayerFSM context) : base(context)
    {
    }

    public override void EnterState()
    {

    }

    public override void UpdateState()
    {
        Context.Move();
        CheckSwitchStates();
    }

    public override void CheckSwitchStates()
    {
        if (!Context.IsAcive) SwitchState<InactiveState>();
        else if (!Context.IsMovePressed && !Context.IsMoving) SwitchState<IdleState>();
    }

    public override void ExitState()
    {

    }
}
