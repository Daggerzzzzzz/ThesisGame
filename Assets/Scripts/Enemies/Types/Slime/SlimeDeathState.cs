public class SlimeDeathState : EnemyState
{
    protected EnemySlime enemy;

    public SlimeDeathState(Enemy enemyBase, EnemyStateMachine stateMachineState, string animationNameState, EnemySlime enemy) : base(enemyBase, stateMachineState, animationNameState)
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
        enemy.SetZeroVelocity();
        enemy.OnCapsuleCollider2D.enabled = false;
    }

    public override void Exit()
    {
        base.Exit();
    }
}
