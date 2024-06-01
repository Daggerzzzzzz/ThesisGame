public class SlimeMoveState : EnemyState
{
    protected EnemySlime enemy;

    public SlimeMoveState(Enemy enemyBase, EnemyStateMachine stateMachineState, string animationNameState, EnemySlime enemy) : base(enemyBase, stateMachineState, animationNameState)
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
