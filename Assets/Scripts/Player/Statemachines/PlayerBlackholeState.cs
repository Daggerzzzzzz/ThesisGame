using System.Collections;
using System.Collections.Generic;
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
        skillUsed = false;
    }

    public override void Update()
    {
        base.Update();
        if (!skillUsed)
        {
            if (player.OnSkill.Blackhole.CanUseSkill())
            {
                skillUsed = true;
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void PlayerAnimationFinishTrigger()
    {
        base.PlayerAnimationFinishTrigger();
    }
}
