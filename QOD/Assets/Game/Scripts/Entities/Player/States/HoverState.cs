using UnityEngine;

public class HoverState : BaseState<PlayerFSM>
{
    public HoverState(PlayerFSM context) : base(context)
    {
    }

    public override void EnterState()
    {
        Debug.Log("Entering Hover State");
        Context.Hover();
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    public override void CheckSwitchStates()
    {
        if (!Context.IsAcive) SwitchState<InactiveState>();
        else if (!Context.IsHoverPressed && !Context.IsMoving) SwitchState<IdleState>();
    }

    public override void ExitState()
    {
        Context.Hover();
        Debug.Log("Exiting Hover State");
    }
}
