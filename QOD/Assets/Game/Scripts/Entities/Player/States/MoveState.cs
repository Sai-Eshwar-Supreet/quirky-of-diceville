using UnityEngine;

public class MoveState : BaseState<PlayerFSM>
{
    public MoveState(PlayerFSM context) : base(context)
    {
    }

    public override void EnterState()
    {
        Debug.Log("Entering Move State");
    }

    public override void UpdateState()
    {
        Context.Move(new Vector3(Context.MoveInput.x, 0, Context.MoveInput.y));
        CheckSwitchStates();
    }

    public override void CheckSwitchStates()
    {
        if (!Context.IsAcive) SwitchState<InactiveState>();
        else if (!Context.IsMovePressed && !Context.IsMoving) SwitchState<IdleState>();
    }

    public override void ExitState()
    {
        Debug.Log("Exiting Move State");
    }
}
