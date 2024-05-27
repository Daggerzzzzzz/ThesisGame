using UnityEngine;

public class PlayerMoveState : PlayerState
{
    public PlayerMoveState(Player playerState, PlayerStateMachine stateMachineState, string animationNameState) : base(playerState, stateMachineState, animationNameState)
    {
        
    }

    public override void Enter()
    {
        base.Enter();
        SoundManager.Instance.PlaySoundEffects(14, null);
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
            player.AttackDirection(movementInput);
            player.attackCheck.transform.localPosition = player.OnAttackDirection;
        }
    }

    public override void Exit()
    {
        base.Exit();
        SoundManager.Instance.StopSoundEffects(14);
    }
}
