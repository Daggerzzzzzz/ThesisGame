using UnityEngine;

public class VastoLordeBattleState : EnemyState
{
    protected EnemyVastoLorde enemy;

    public VastoLordeBattleState(Enemy enemyBase, EnemyStateMachine stateMachineState, string animationNameState, EnemyVastoLorde enemy) : base(enemyBase, stateMachineState, animationNameState)
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
        
        if (enemy.CanDoSpell())
        {
            stateMachine.ChangeState(enemy.OnCastState);
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
