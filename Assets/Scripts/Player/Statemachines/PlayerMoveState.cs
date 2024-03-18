using UnityEngine;

public class PlayerMoveState : PlayerState
{
    private static readonly int MoveX = Animator.StringToHash("moveX");
    private static readonly int MoveY = Animator.StringToHash("moveY");
    
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
