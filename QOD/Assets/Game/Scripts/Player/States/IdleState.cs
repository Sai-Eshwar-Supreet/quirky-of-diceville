using UnityEngine;

public class IdleState : BaseState<PlayerFSM>
{
    public IdleState(PlayerFSM context) : base(context)
    {
    }

    public override void EnterState()
    {
        Debug.Log("Entering Idle State");
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    public override void CheckSwitchStates()
    {
        if (Context.IsMoving) return;
        if(!Context.IsAcive) SwitchState<InactiveState>();
        else if (Context.IsHoverPressed) SwitchState<HoverState>();
        else if (Context.IsMovePressed) SwitchState<MoveState>();
    }

    public override void ExitState()
    {
        Debug.Log("Exiting Idle State");
    }
}
