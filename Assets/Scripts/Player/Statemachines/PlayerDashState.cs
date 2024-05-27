using UnityEngine;

public class PlayerDashState : PlayerState
{
    public PlayerDashState(Player playerState, PlayerStateMachine stateMachineState, string animationNameState) : base(playerState, stateMachineState, animationNameState)
    {
        
    }
    
    public override void Enter()
    {
        base.Enter();
        SoundManager.Instance.PlaySoundEffects(10, null);
        player.OnPlayerInputs.Player.Move.Disable();
        player.OnSkill.Dash.CloneDash();
        stateTimer = player.dashDuration;
    }

    public override void Update()
    {
        base.Update();
        player.SetVelocity(player.dashSpeed * player.dashDirection.x, player.dashSpeed * player.dashDirection.y, player.moveSpeed);
        if(Mathf.Abs(player.transform.position.x - player.lastImageXpos) > distanceBetweenImages)
        {
            PlayerAfterImagePool.Instance.GetFromPool();
            player.lastImageXpos = player.transform.position.x;
        }
        if(Mathf.Abs(player.transform.position.y - player.lastImageYpos) > distanceBetweenImages)
        {
            PlayerAfterImagePool.Instance.GetFromPool();
            player.lastImageYpos = player.transform.position.y;
        }
        if (stateTimer < 0)
        {
            stateMachine.ChangeState(player.OnIdleState);
        }    
    }

    public override void Exit()
    {
        base.Exit();
        SoundManager.Instance.StopSoundEffects(10);
        player.OnPlayerInputs.Player.Move.Enable();
        player.OnSkill.Dash.DoubleCloneDash();
        player.SetVelocity(0, 0, player.moveSpeed);
        player.dashDirection = Vector2.zero;
    }
}

