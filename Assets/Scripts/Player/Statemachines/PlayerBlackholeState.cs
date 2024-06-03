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
        player.OnRb.constraints = RigidbodyConstraints2D.FreezeAll;
        player.OnPlayerInputs.Player.Disable();
        skillUsed = false;
    }

    public override void Update()
    {
        base.Update();
        player.OnRb.constraints = RigidbodyConstraints2D.FreezeAll;
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
        player.OnPlayerInputs.Player.Enable();
        player.OnRb.constraints = RigidbodyConstraints2D.None;
        player.OnRb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
}
