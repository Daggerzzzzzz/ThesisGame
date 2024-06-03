using UnityEngine;

public class PlayerAimState : PlayerState
{
    public PlayerAimState(Player playerState, PlayerStateMachine stateMachineState, string animationNameState) : base(playerState, stateMachineState, animationNameState)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.OnPlayerInputs.Player.Fire.Disable();
        player.OnPlayerInputs.Player.Dash.Disable();
    }

    public override void Update()
    {
        base.Update();
        rb.velocity = Vector2.zero;
        if (movementInput != Vector2.zero)
        {
            player.SetAnimator(movementInput);
        }
        if ( player.OnPlayerInputs.Player.ThrowSword.WasReleasedThisFrame())
        {
            stateMachine.ChangeState(player.OnIdleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        player.OnPlayerInputs.Player.Fire.Enable();
        player.OnPlayerInputs.Player.Dash.Enable();
    }
}
