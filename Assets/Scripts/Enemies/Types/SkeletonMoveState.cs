using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonMoveState : SkeletonState
{
    public SkeletonMoveState(Enemy enemyBase, EnemyStateMachine stateMachineState, string animationNameState, EnemySkeleton _enemy) : base(enemyBase, stateMachineState, animationNameState, _enemy)
    {
        
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        //enemy.
    }

    public override void Exit()
    {
        base.Exit();
    }
}
