using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBattleState : EnemyState
{
    private EnemySkeleton enemy;
    private Vector2 skeletonDirection;
    
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
        enemyMovementInput = new Vector2(enemy.EnemyDirection.x, enemy.EnemyDirection.y);
        if (enemyMovementInput != null)
        {
            enemy.SetVelocity(enemyMovementInput.x, enemyMovementInput.y, enemy.moveSpeed);
            skeletonDirection = new Vector2(enemyMovementInput.x, enemyMovementInput.y);

            if (skeletonDirection != Vector2.zero)
            {
                AttackDirection();
                
                if (enemy.attackCheck != null)
                {
                    enemy.attackCheck.transform.localPosition = attackDirection;
                }
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
