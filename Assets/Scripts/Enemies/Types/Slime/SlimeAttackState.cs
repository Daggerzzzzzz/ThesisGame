using UnityEngine;

public class SlimeAttackState : EnemyState
{
    protected EnemySlime enemy;

    public SlimeAttackState(Enemy enemyBase, EnemyStateMachine stateMachineState, string animationNameState, EnemySlime enemy) : base(enemyBase, stateMachineState, animationNameState)
    {
        this.enemy = enemy;
    }

    public override void Update()
    {
        base.Update();
        enemy.SetZeroVelocity();

        if (triggerCalled)
        {
            stateMachine.ChangeState(enemy.OnBattleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        enemy.lastTimeAttacked = Time.time;
    }
}
