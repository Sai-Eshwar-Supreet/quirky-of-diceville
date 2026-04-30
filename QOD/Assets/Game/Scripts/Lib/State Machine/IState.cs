public interface IState
{
    /// <summary>
    /// Switch to a new state.
    /// </summary>
    /// <typeparam name="T">The type of the new state. Must be a sub class of IState.</typeparam>
    public void SwitchState<T>() where T : IState;
    /// <summary>
    /// Called when the state is used or entered.
    /// </summary>
    public void EnterState();

    /// <summary>
    /// Called once per frame when the state is active.
    /// </summary>
    public void UpdateState();

    /// <summary>
    /// State switching logic goes here.
    /// </summary>
    public void CheckSwitchStates();
    /// <summary>
    /// Called when the state is exited or deactivated.
    /// </summary>
    public void ExitState();
}