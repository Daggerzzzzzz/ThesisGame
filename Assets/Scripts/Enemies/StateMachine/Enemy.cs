using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : Entity
{
    [Header("Movement Inputs")]
    public float moveSpeed = 5f;
    public float idleTime;
    private float defaultMovementSpeed;

    [Header("Enemy Details")] 
    public EnemyDataSO enemyDataSo;
    private MaterializeEffect materializeEffect;
    private BoxCollider2D boxCollider2D;
    private CapsuleCollider2D capsuleCollider2D;
    private EnemyAI enemyAI;
    
    public EnemyStateMachine OnStateMachine { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        OnStateMachine = new EnemyStateMachine();
        materializeEffect = GetComponent<MaterializeEffect>();
        enemyAI = GetComponent<EnemyAI>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        defaultMovementSpeed = moveSpeed;
    }

    protected override void Start()
    {
        base.Start();
        StartCoroutine(MaterializeEnemy());
    }

    protected override void Update()
    {
        base.Update();
        OnStateMachine.OnCurrentState.Update();
    }
    
    private IEnumerator MaterializeEnemy()
    {
        EnemyEnable(false);
        yield return StartCoroutine(materializeEffect.MaterializeRoutine(enemyDataSo.enemyMaterializeTime, enemyDataSo.enemyStandardMaterial));
        EnemyEnable(true);
    }

    private void EnemyEnable(bool check)
    {
        boxCollider2D.enabled = check;
        capsuleCollider2D.enabled = check;
        enemyAI.enabled = check;
    }

    public virtual void TimeFreeze(bool timeFrozen)
    {
        if (timeFrozen)
        {
            moveSpeed = 0;
            OnAnim.speed = 0;
        }
        else
        {
            moveSpeed = defaultMovementSpeed;
            OnAnim.speed = 1;
        }
    }

    protected virtual IEnumerator FreezeTimeFor(float seconds)
    {
        TimeFreeze(true);
        yield return new WaitForSeconds(seconds);
        TimeFreeze(false);
    }
}
