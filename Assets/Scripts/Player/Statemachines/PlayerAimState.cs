using UnityEngine;

public class PlayerAimState : PlayerState
{
    public PlayerAimState(Player playerState, PlayerStateMachine stateMachineState, string animationNameState) : base(playerState, stateMachineState, animationNameState)
    {
    }

    public override void Enter()
    {
        base.Enter();
        playerInputs.Player.Fire.Disable();
        playerInputs.Player.Dash.Disable();
    }

    public override void Update()
    {
        base.Update();
        rb.velocity = Vector2.zero;
        if (movementInput != Vector2.zero)
        {
            player.SetAnimator(movementInput);
        }
        if (playerInputs.Player.ThrowSword.WasReleasedThisFrame())
        {
            stateMachine.ChangeState(player.OnIdleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        playerInputs.Player.Fire.Enable();
        playerInputs.Player.Dash.Enable();
    }
}
