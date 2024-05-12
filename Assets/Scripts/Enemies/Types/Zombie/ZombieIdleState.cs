public class ZombieIdleState : EnemyState
{
    private EnemyZombie enemy;
    
    public ZombieIdleState(Enemy enemyBase, EnemyStateMachine stateMachineState, string animationNameState, EnemyZombie enemy) : base(enemyBase, stateMachineState, animationNameState)
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
