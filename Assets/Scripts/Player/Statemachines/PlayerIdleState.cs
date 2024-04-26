using UnityEditor.Timeline.Actions;
using UnityEngine;

public class PlayerIdleState : PlayerState
{
    public PlayerIdleState(Player playerState, PlayerStateMachine stateMachineState, string animationNameState) : base(playerState, stateMachineState, animationNameState)
    {
        
    }

    public override void Enter()
    {
        base.Enter();
        player.SetZeroVelocity();
    }

    public override void Update()
    {
        base.Update();
        if (movementInput != Vector2.zero && !player.OnIsBusy)
        {
            stateMachine.ChangeState(player.OnMoveState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
