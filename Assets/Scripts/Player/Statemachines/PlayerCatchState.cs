using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCatchState : PlayerState
{
    private static readonly int Back = Animator.StringToHash("back");
    private static readonly int Front = Animator.StringToHash("front");
    private static readonly int Left = Animator.StringToHash("left");
    private static readonly int Right = Animator.StringToHash("right");
    
    private Transform sword;
    private Vector2 difference;
    
    public PlayerCatchState(Player playerState, PlayerStateMachine stateMachineState, string animationNameState) : base(playerState, stateMachineState, animationNameState)
    {
    }
    
    public override void Enter()
    {
        base.Enter();
        sword = player.OnSword.transform;

        if (player.transform.position.x > sword.transform.position.x)
        {
            player.OnAnim.SetBool(Left, true);
            rb.velocity = new Vector2(10 * 1, rb.velocity.y);
        }
        else if (player.transform.position.x < sword.transform.position.x)
        {
            player.OnAnim.SetBool(Right, true);
            rb.velocity = new Vector2(10 * -1, rb.velocity.y);
        }
        else if (player.transform.position.y > sword.transform.position.y)
        {
            player.OnAnim.SetBool(Front, true);
            rb.velocity = new Vector2(rb.velocity.x, 10 * 1);
        }
        else if (player.transform.position.y < sword.transform.position.y)
        {
            player.OnAnim.SetBool(Back, true);
            rb.velocity = new Vector2(rb.velocity.x, 10 * -1);
        }
        else
        {
            player.OnAnim.SetBool(Front, true);
            rb.velocity = new Vector2(rb.velocity.x, 10 * 1);
        }
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
        ResetAnimator();
    }

    private void ResetAnimator()
    {
        player.OnAnim.SetBool(Left, false);
        player.OnAnim.SetBool(Right, false);
        player.OnAnim.SetBool(Front, false);
        player.OnAnim.SetBool(Back, false);
    }
}
