using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : Entity
{
    [Header("Movement Inputs")]
    public float moveSpeed = 5f;
    public float idleTime;
    
    public EnemyStateMachine OnStateMachine { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        OnStateMachine = new EnemyStateMachine();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        OnStateMachine.OnCurrentState.Update();
    }
    
}
