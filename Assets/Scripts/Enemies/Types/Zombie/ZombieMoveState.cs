public class ZombieMoveState : EnemyState
{
    private EnemyZombie enemy;
    
    public ZombieMoveState(Enemy enemyBase, EnemyStateMachine stateMachineState, string animationNameState, EnemyZombie enemy) : base(enemyBase, stateMachineState, animationNameState)
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
