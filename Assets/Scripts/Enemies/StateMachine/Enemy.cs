using System;
using System.Collections;
using UnityEngine;

public class Enemy : Entity
{
    [Header("Movement Inputs")]
    public float moveSpeed = 5f;
    public float idleTime;
    private float defaultMovementSpeed;

    [Header("Attack Inputs")] 
    public float attackCooldown;
    [HideInInspector]
    public float lastTimeAttacked;

    [Header("Enemy Details")] 
    public EnemyDataSO enemyDataSo;
    private MaterializeEffect materializeEffect;
    private BoxCollider2D boxCollider2D;
    private CapsuleCollider2D capsuleCollider2D;
    private EnemyAI enemyAI;

    protected EnemyStateMachine OnStateMachine { get; private set; }
    public bool OnIsPlayerFollowing { get; private set; }
    public bool OnIsPlayerAttacking { get; private set; }
    public Vector2 EnemyDirection { get; private set; }

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
    
    public void PlayerFollowCheck(bool isFollowing)
    {
        OnIsPlayerFollowing = isFollowing;
    }
    
    public void PlayerAttackCheck(bool isAttacking)
    {
        OnIsPlayerAttacking = isAttacking;
    }
    
    public void EnemyMovement(Vector2 movementInput)
    {
        EnemyDirection = movementInput;
    }
    
    public void AnimationTriggerForEnemy()
    {
        OnStateMachine.OnCurrentState.EnemyAnimationFinishTrigger();
    }

    public override void SlowEntityBy(float slowPercent, float slowDuration)
    {
        moveSpeed *= (1 - slowPercent);
        OnAnim.speed *= (1 - slowPercent);
        
        Invoke(nameof(returnToNormalSpeed), slowDuration);
    }

    protected override void returnToNormalSpeed()
    {
        base.returnToNormalSpeed();
        moveSpeed = defaultMovementSpeed;
    }
}
