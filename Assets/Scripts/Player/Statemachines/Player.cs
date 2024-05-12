using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    [Header("Dash Inputs")] 
    [SerializeField]
    private float distance;
    [HideInInspector]
    public Vector2 dashDirection;
    private const float DashCheckRadius = 1f;
    public float dashSpeed;
    public float dashDuration;
    private float normalDashSpeed;
    
    [Header("Clone Inputs")] 
    private RaycastHit2D dashHit;
    public Collider2D OnEnemyDashedCollider { get; private set; }

    
    [Header("Movement Inputs")]
    public float moveSpeed = 10f;
    private Transform playerTransform;
    private float normalMoveSpeed;

    [Header("Equipment Tooltip")] 
    public GameObject eKey;
    public GameObject equipmentInfo;
    public ItemTooltip itemTooltip;
    
    public static Transform OnTransformPosition { get; private set; }
    public SkillManager OnSkill { get; private set; }
    public GameObject OnSword { get; private set; }

    #region States
    public PlayerStateMachine OnStateMachine { get; private set; } 
    public PlayerIdleState OnIdleState { get; private set; }
    public PlayerMoveState OnMoveState { get; private set; }
    public PlayerDashState OnDashState { get; private set; }
    public PlayerPrimaryAttackState OnPrimaryAttackState { get; private set; }
    public PlayerAimState OnPlayerAimState { get; private set; }
    public PlayerCatchState OnPlayerCatchState { get; private set; }
    public PlayerBlackholeState OnPlayerBlackholeState { get; private set; }
    public PlayerDeathState OnDeathState { get; private set; }

    #endregion
    
    [HideInInspector]
    public float lastImageXpos;
    [HideInInspector]
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
        OnPlayerAimState = new PlayerAimState(this, OnStateMachine, "aimSword");
        OnPlayerCatchState = new PlayerCatchState(this, OnStateMachine, "catchSword");
        OnPlayerBlackholeState = new PlayerBlackholeState(this, OnStateMachine, "idle");
        OnDeathState = new PlayerDeathState(this, OnStateMachine, "death");
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

        normalMoveSpeed = moveSpeed;
        normalDashSpeed = dashSpeed;
    }

    protected override void Update()
    {
        base.Update();
        OnStateMachine.OnCurrentState.Update();
        CheckForDashInput();
        OnTransformPosition = playerTransform;

        if (OnPlayerInputs.Player.Teleport.IsPressed() && OnSkill.Kunai.KunaiUnlocked)
        {
            OnPlayerInputs.Player.Teleport.Disable();
            OnSkill.Kunai.CanUseSkill();
            if (OnSkill.Kunai.KunaiStackExplodeUnlocked)
            {
                OnPlayerInputs.Player.Teleport.Enable();
            }
        }
    }

    public void OnDisable()
    {
        OnPlayerInputs.Disable();
    }
    
    public void NewSwordAssignment(GameObject newSword)
    {
        OnSword = newSword;
    }

    public void ClearNewSword()
    {
        OnStateMachine.ChangeState(OnPlayerCatchState);
        Destroy(OnSword);
    }

    private void CheckForDashInput()
    {
        if (OnSkill.Dash.DashUnlocked == false)
        {
            return;
        }
        
        if (OnPlayerInputs.Player.Dash.IsPressed() && SkillManager.Instance.Dash.CanUseSkill())
        {
            if (transform != null)
            {
                dashDirection = new Vector2(OnMovementDirection.x, OnMovementDirection.y).normalized;
                PlayerAfterImagePool.Instance.GetFromPool();
                lastImageXpos = transform.position.x;
                lastImageYpos = transform.position.y;

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

    public void AnimationTriggerForPlayer()
    {
        OnStateMachine.OnCurrentState.PlayerAnimationFinishTrigger();
    }

    public override void EntityDeath()
    {
        base.EntityDeath();
        OnStateMachine.ChangeState(OnDeathState);
    }

    public override void SlowEntityBy(float slowPercent, float slowDuration)
    {
        moveSpeed *= (1 - slowPercent);
        dashSpeed *= (1 - slowPercent);
        OnAnim.speed *= (1 - slowPercent);
        
        Invoke(nameof(ReturnToNormalSpeed), slowDuration);
    }

    protected override void ReturnToNormalSpeed()
    {
        base.ReturnToNormalSpeed();

        moveSpeed = normalMoveSpeed;
        dashSpeed = normalDashSpeed;
    }

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

