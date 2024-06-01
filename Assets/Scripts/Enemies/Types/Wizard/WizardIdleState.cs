public class WizardIdleState : EnemyState
{
    private EnemyWizard enemy;

    public WizardIdleState(Enemy enemyBase, EnemyStateMachine stateMachineState, string animationNameState, EnemyWizard enemy) : base(enemyBase, stateMachineState, animationNameState)
    {
        this.enemy = enemy;
    }
    
    public override void Enter()
    {
        base.Enter();
        stateTimer = enemy.idleTime;
    }

    public override void Update()
    {
        base.Update();
        
        if (enemy.OnIsPlayerFollowing)
        {
            stateMachine.ChangeState(enemy.OnBattleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
