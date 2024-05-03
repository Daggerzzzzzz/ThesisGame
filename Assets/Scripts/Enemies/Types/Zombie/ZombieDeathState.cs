using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieDeathState : EnemyState
{
    private EnemyZombie enemy;

    public ZombieDeathState(Enemy enemyBase, EnemyStateMachine stateMachineState, string animationNameState, EnemyZombie enemy) : base(enemyBase, stateMachineState, animationNameState)
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
        enemy.SetZeroVelocity();
        enemy.OnCapsuleCollider2D.enabled = false;
    }

    public override void Exit()
    {
        base.Exit();
    }
}
