using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemySkeleton : Enemy
{
    #region States
    public SkeletonIdleState idleState { get; private set; }
    public SkeletonMoveState moveState { get; private set; }
    public SkeletonBattleState battleState { get; private set; }
    #endregion

    public static bool OnIsPlayerFollowing { get; private set; }
    public Vector2 EnemyDirection { get; private set; }
    
    protected override void Awake()
    {
        base.Awake();
        idleState = new SkeletonIdleState(this, OnStateMachine, "idle", this);
        moveState = new SkeletonMoveState(this, OnStateMachine, "walk", this);
        battleState = new SkeletonBattleState(this, OnStateMachine, "walk", this);
    }

    protected override void Start()
    {
        base.Start();
        OnStateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();
    }

    public void PlayerFollowCheck(bool isFollowing)
    {
        OnIsPlayerFollowing = isFollowing;
    }
    
    public void EnemyMovement(Vector2 movementInput)
    {
        EnemyDirection = movementInput;
    }
}
