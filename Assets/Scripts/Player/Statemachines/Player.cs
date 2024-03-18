using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    [Header("Dash Inputs")] 
    [SerializeField]
    private float distance;

    private const float DashCheckRadius = 1f;
    public float dashSpeed;
    public float dashDuration;
    public bool isDashing;
    public Vector2 dashDirection;
    
    [Header("Clone Inputs")] 
    private RaycastHit2D dashHit;
    public Collider2D OnEnemyDashedCollider { get; private set; }
    public bool OnCanCreateClone { get; internal set; }

    
    [Header("Movement Inputs")]
    public float moveSpeed = 10f;
    [SerializeField]
    private Transform playerTransform;
    
    public static Transform OnTransformPosition { get; private set; }
    public SkillManager OnSkill { get; private set; }
    
    #region States
    public PlayerStateMachine OnStateMachine { get; private set; } 
    public PlayerIdleState OnIdleState { get; private set; }
    public PlayerMoveState OnMoveState { get; private set; }
    public PlayerDashState OnDashState { get; private set; }
    public PlayerPrimaryAttackState OnPrimaryAttackState { get; private set; }
    #endregion
    
    public float lastImageXpos;
    public float lastImageYpos;
    public bool OnIsBusy { get; private set; }
    public PlayerInputs OnPlayerInputs { get; private set; }
    
    protected override void Awake()
    {
        base.Awake();
        OnPlayerInputs = new PlayerInputs();
        OnStateMachine = new PlayerStateMachine();
        OnIdleState = new PlayerIdleState(this, OnStateMachine, "idle");
        OnMoveState = new PlayerMoveState(this, OnStateMachine, "walk");
        OnDashState = new PlayerDashState(this, OnStateMachine, "dash");
        OnPrimaryAttackState = new PlayerPrimaryAttackState(this, OnStateMachine, "attack");
    }

    public void OnEnable()
    {
        OnPlayerInputs.Enable();
    }

    protected override void Start()
    {
        base.Start();
        OnSkill = SkillManager.Instance;
        OnStateMachine.Initialize(OnIdleState);
    }

    protected override void Update()
    {
        base.Update();
        OnStateMachine.OnCurrentState.Update();
        CheckForDashInput();
        OnTransformPosition = playerTransform;
    }

    public void OnDisable()
    {
        OnPlayerInputs.Disable();
    }

    private void CheckForDashInput()
    {
        if (Input.GetKeyDown(KeyCode.Space) && SkillManager.Instance.dash.CanUseSkill())
        {
            isDashing = true;
            if (transform != null)
            {
                dashDirection = new Vector2(movementDirection.x, movementDirection.y).normalized;
                PlayerAfterImagePool.Instance.GetFromPool();
                lastImageXpos = transform.position.x;
                lastImageYpos = transform.position.y;

                dashHit = Physics2D.CircleCast(transform.position, DashCheckRadius, dashDirection * dashSpeed);
                
                if (dashHit.collider != null && dashHit.collider.CompareTag("Movement Collider"))
                {
                    Debug.Log("Enemy Hit");
                    OnCanCreateClone = true;
                    OnEnemyDashedCollider = dashHit.collider;
                }

                if (dashDirection != Vector2.zero)
                {
                    OnStateMachine.ChangeState(OnDashState);
                }
            }
        }
    }
    
    public IEnumerator BusyFor(float seconds)
    {
        OnIsBusy = true;
        yield return new WaitForSeconds(seconds);
        OnIsBusy = false;
    }

    public void AnimationTriggerForPlayer() => OnStateMachine.OnCurrentState.PlayerAnimationFinishTrigger();
    
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelperUtilities.ValidateCheckNullValue(this, nameof(OnTransformPosition), OnTransformPosition);
        HelperUtilities.ValidateCheckNullValue(this, nameof(OnEnemyDashedCollider), OnEnemyDashedCollider);
        HelperUtilities.ValidateCheckNullValue(this, nameof(playerTransform), playerTransform);
        HelperUtilities.ValidateCheckNullValue(this, nameof(OnSkill), OnSkill);
    }
#endif
}

