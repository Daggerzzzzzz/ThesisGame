using UnityEngine;

public class SlimeBattleState : EnemyState
{
    protected EnemySlime enemy;

    public SlimeBattleState(Enemy enemyBase, EnemyStateMachine stateMachineState, string animationNameState, EnemySlime enemy) : base(enemyBase, stateMachineState, animationNameState)
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
        
        if (enemy.OnIsPlayerAttacking)
        {
            if (CanAttack())
            {
                stateMachine.ChangeState(enemy.OnAttackState);
            }
        }
        
        if (!enemy.OnIsPlayerFollowing)
        {
            stateMachine.ChangeState(enemy.OnIdleState);
        }
        
        enemy.SetVelocity(enemy.EnemyDirection.x, enemy.EnemyDirection.y, enemy.moveSpeed);
        enemy.AttackDirection(enemy.EnemyDirection);
        enemy.attackCheck.transform.localPosition = enemy.OnAttackDirection;
    }

    public override void Exit()
    {
        base.Exit();
    }

    private bool CanAttack()
    {
        if (Time.time >= enemy.lastTimeAttacked + enemy.attackCooldown)
        {
            enemy.lastTimeAttacked = Time.time;
            return true;
        }
        
        return false;
    }
}
