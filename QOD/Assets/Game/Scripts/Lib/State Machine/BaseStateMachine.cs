using UnityEngine;

public abstract class BaseStateMachine : MonoBehaviour
{
    public StateFactory StateFactory { get; protected set; }

    public IState CurrentState { get; set; }



    protected virtual void Awake()
    {
        StateFactory = new StateFactory();

        InitializeStates();

        CurrentState?.EnterState();
    }

    /// <summary>
    /// Initialize the states of the state machine here.
    /// </summary>
    public abstract void InitializeStates();

    protected virtual void Update()
    {
        CurrentState?.UpdateState();
    }
}