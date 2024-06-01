using UnityEngine;

public class VastoLordeAttackState : EnemyState
{
    protected EnemyVastoLorde enemy;

    public VastoLordeAttackState(Enemy enemyBase, EnemyStateMachine stateMachineState, string animationNameState, EnemyVastoLorde enemy) : base(enemyBase, stateMachineState, animationNameState)
    {
        this.enemy = enemy;
    }
    
    public override void Enter()
    {
        base.Enter();
        enemy.chanceToTeleport += 5;
    }

    public override void Update()
    {
        base.Update();
        enemy.SetZeroVelocity();

        if (triggerCalled)
        {
            if (enemy.CanTeleport())
            {
                stateMachine.ChangeState(enemy.OnTeleportState);
            }
            else
            {
                stateMachine.ChangeState(enemy.OnBattleState);
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
        enemy.lastTimeAttacked = Time.time;
    }
}
