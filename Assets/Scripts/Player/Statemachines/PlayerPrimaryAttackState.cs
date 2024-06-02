using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{
    private int comboCounter;
    
    private float lastTimeAttack;
    private float comboWindow = 2;

    private PlayerInputs playerNewInputs;
    
    private static readonly int ComboCounter = Animator.StringToHash("comboCounter");

    public PlayerPrimaryAttackState(Player playerState, PlayerStateMachine stateMachineState, string animationNameState) : base(playerState, stateMachineState, animationNameState)
    {
        
    }
    public override void Enter()
    {
        base.Enter();

        if (comboCounter > 2 || Time.time >= lastTimeAttack + comboWindow)
        {
            comboCounter = 0;
        }
        
        playerNewInputs = player.OnPlayerInputs;
        playerNewInputs.Player.Fire.Disable();
        player.OnAnim.SetInteger(ComboCounter, comboCounter);
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
        
        
        playerNewInputs.Player.Fire.Enable();

        comboCounter++;
        lastTimeAttack = Time.time;
    }
    
}
