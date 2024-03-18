using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonState : EnemyState
{
    protected EnemySkeleton enemy;
    protected SkeletonState(Enemy enemyBase, EnemyStateMachine stateMachineState, string animationNameState, EnemySkeleton _enemy) : base(enemyBase, stateMachineState, animationNameState)
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
        if (EnemySkeleton.OnIsPlayerFollowing)
        {
            stateMachine.ChangeState(enemy.battleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        
    }
}
