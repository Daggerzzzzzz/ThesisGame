public class PlayerPrimaryAttackState : PlayerState
{
    private int comboCounter;
    private float lastTimeAttack;

    private PlayerInputs playerNewInputs;
        
    public PlayerPrimaryAttackState(Player playerState, PlayerStateMachine stateMachineState, string animationNameState) : base(playerState, stateMachineState, animationNameState)
    {
        
    }
    public override void Enter()
    {
        base.Enter();
        playerNewInputs = player.OnPlayerInputs;
        playerNewInputs.Player.Fire.Disable();
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
    }
    
}
