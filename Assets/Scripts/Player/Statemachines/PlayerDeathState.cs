using Unity.VisualScripting;
using UnityEngine;

public class PlayerDeathState : PlayerState
{
    public PlayerDeathState(Player playerState, PlayerStateMachine stateMachineState, string animationNameState) : base(playerState, stateMachineState, animationNameState)
    {
        
    }

    public override void Enter()
    {
        base.Enter();
        player.OnPlayerInputs.Player.Disable();
        
        GameObject.Find("Pause").GetComponent<UI>().EnableEndScreen();
    }

    public override void Update()
    {
        base.Update();
        player.SetZeroVelocity();
        player.OnCapsuleCollider2D.enabled = false;
        player.OnBoxCollider2D.enabled = false;
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
