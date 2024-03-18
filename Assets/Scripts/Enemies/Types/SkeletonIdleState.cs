using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonIdleState : SkeletonState
{
    public SkeletonIdleState(Enemy enemyBase, EnemyStateMachine stateMachineState, string animationNameState, EnemySkeleton _enemy) : base(enemyBase, stateMachineState, animationNameState, _enemy)
    {
        
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = enemy.idleTime;
    }

    public override void Update()
    {
        base.Update();
        
    }

    public override void Exit()
    {
        base.Exit();
    }
}
