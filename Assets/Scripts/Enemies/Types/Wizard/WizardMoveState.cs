public class WizardMoveState : EnemyState
{
    private EnemyWizard enemy;

    public WizardMoveState(Enemy enemyBase, EnemyStateMachine stateMachineState, string animationNameState, EnemyWizard enemy) : base(enemyBase, stateMachineState, animationNameState)
    {
        this.enemy = enemy;
    }
    
    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
