using UnityEngine;

public class PlayerBlackholeState : PlayerState
{
    private bool skillUsed;
    
    public PlayerBlackholeState(Player playerState, PlayerStateMachine stateMachineState, string animationNameState) : base(playerState, stateMachineState, animationNameState)
    {
        
    }

    public override void Enter()
    {
        base.Enter();
        player.OnRb.constraints = RigidbodyConstraints2D.FreezePosition;
        playerInputs.Player.Disable();
        skillUsed = false;
    }

    public override void Update()
    {
        base.Update();
        player.OnRb.constraints = RigidbodyConstraints2D.FreezeRotation;
        if (!skillUsed)
        {
            if (player.OnSkill.Blackhole.CanUseSkill())
            {
                skillUsed = true;
            }
        }

        if (player.OnSkill.Blackhole.BlackholeIsFinished())
        {
            stateMachine.ChangeState(player.OnIdleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        playerInputs.Player.Enable();
        player.OnRb.constraints = RigidbodyConstraints2D.None;
        player.OnRb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public override void PlayerAnimationFinishTrigger()
    {
        base.PlayerAnimationFinishTrigger();
    }
}
