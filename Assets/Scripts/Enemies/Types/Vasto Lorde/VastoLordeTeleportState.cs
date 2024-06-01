public class VastoLordeTeleportState : EnemyState
{
    protected EnemyVastoLorde enemy;
    
    public VastoLordeTeleportState(Enemy enemyBase, EnemyStateMachine stateMachineState, string animationNameState, EnemyVastoLorde enemy) : base(enemyBase, stateMachineState, animationNameState)
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

        if(triggerCalled)
        {
            stateMachine.ChangeState(enemy.OnBattleState);
        }
    }
}
