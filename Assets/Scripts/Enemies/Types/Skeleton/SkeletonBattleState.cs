using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBattleState : EnemyState
{
    private EnemySkeleton enemy;
    
    public SkeletonBattleState(Enemy enemyBase, EnemyStateMachine stateMachineState, string animationNameState, EnemySkeleton _enemy) : base(enemyBase, stateMachineState, animationNameState)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        
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
}
