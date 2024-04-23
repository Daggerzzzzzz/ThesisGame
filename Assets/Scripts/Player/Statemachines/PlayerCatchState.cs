using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCatchState : PlayerState
{
    private static readonly int CatchX = Animator.StringToHash("catchX");
    private static readonly int CatchY = Animator.StringToHash("catchY");
    
    private Rigidbody2D swordRb;
    private Vector2 difference;
    
    public PlayerCatchState(Player playerState, PlayerStateMachine stateMachineState, string animationNameState) : base(playerState, stateMachineState, animationNameState)
    {
    }

    public override void Enter()
    {
        base.Enter();
        swordRb = player.OnSword.GetComponent<Rigidbody2D>();

        difference = swordRb.transform.position - player.transform.position;
        difference.Normalize();
        player.OnAnim.SetFloat(CatchX, difference.x);
        player.OnAnim.SetFloat(CatchY, difference.y);
        player.OnRb.AddForceAtPosition(-(difference * 5), player.transform.position, ForceMode2D.Impulse);
    }

    public override void Update()
    {
        base.Update();
        if (triggerCalled)
        {
            stateMachine.ChangeState(player.OnIdleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
