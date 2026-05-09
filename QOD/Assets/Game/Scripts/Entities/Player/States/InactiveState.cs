using UnityEngine;

public class InactiveState : BaseState<PlayerFSM>
{
    public InactiveState(PlayerFSM context) : base(context)
    {
    }

    public override void EnterState()
    {

    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    public override void CheckSwitchStates()
    {
        if (Context.IsAcive) SwitchState<IdleState>();
    }

    public override void ExitState()
    {

    }
}
