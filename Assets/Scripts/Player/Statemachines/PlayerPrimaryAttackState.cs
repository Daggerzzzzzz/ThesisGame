using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPrimaryAttackState : PlayerState
{
    private int comboCounter;
    private float lastTimeAttack;
    private float comboWindow = 2;
    private float loopDuration = 0.7f; 
    private bool isInLoop = true;
    
    PlayerInputs playerInputs;
        
    public PlayerPrimaryAttackState(Player playerState, PlayerStateMachine stateMachineState, string animationNameState) : base(playerState, stateMachineState, animationNameState)
    {
        
    }
    public override void Enter()
    {
        base.Enter();
        playerInputs = player.OnPlayerInputs;
        playerInputs.Player.Fire.Disable();
        /*if (comboCounter > 2 || Time.time >= lastTimeAttack + comboWindow)
        {
            comboCounter = 0;
        }*/
        //stateTimer = .1f;
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer < 0)
        {
            player.SetZeroVelocity();
        }
        if (triggerCalled) 
        {
            stateMachine.ChangeState(player.OnIdleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        playerInputs.Player.Fire.Enable();
        //player.StartCoroutine(nameof(Player.BusyFor), .7f);
        //Debug.Log("Exit");
        /*comboCounter ++;
        lastTmeAttack = Time.time;*/
    }
    
}
