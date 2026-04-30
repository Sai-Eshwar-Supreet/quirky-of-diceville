using System;

public abstract class BaseState<T> : IState where T : BaseStateMachine
{
    public BaseState(T context) => Context = context;

    public readonly T Context;
    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void CheckSwitchStates();
    public abstract void ExitState();

    public void SwitchState<S>() where S : IState
    {
        var newState = Context.StateFactory.GetState<S>() ?? throw new InvalidOperationException($"Cannot switch to state {typeof(S).Name} as it's not a valid BaseState<{typeof(T).Name}>");

        ExitState();

        Context.CurrentState = newState;
        Context.CurrentState.EnterState();
    }

}