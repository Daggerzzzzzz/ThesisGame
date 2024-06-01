using UnityEngine;

public class WizardAttackState : EnemyState
{
    protected EnemyWizard enemy;

    public WizardAttackState(Enemy enemyBase, EnemyStateMachine stateMachineState, string animationNameState, EnemyWizard enemy) : base(enemyBase, stateMachineState, animationNameState)
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
