using UnityEngine;

public class PlayerMoveState : PlayerState
{
    public PlayerMoveState(Player playerState, PlayerStateMachine stateMachineState, string animationNameState) : base(playerState, stateMachineState, animationNameState)
    {
        
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        player.SetVelocity(movementInput.x, movementInput.y, player.moveSpeed);
        if (movementInput == Vector2.zero)
        {
            stateMachine.ChangeState(player.OnIdleState);
        }
        else
        {
            AttackDirection();
            player.attackCheck.transform.localPosition = attackDirection;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
