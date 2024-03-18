using UnityEngine;

public class PlayerStateMachine
{
    public PlayerState OnCurrentState { get; private set; } 

    public void Initialize(PlayerState startState)
    {
        OnCurrentState = startState;
        OnCurrentState.Enter();
    }

    public void ChangeState(PlayerState newState)
    {
        OnCurrentState.Exit();
        OnCurrentState = newState;
        OnCurrentState.Enter();
    }
}
