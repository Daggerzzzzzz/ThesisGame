using System.Collections;
using UnityEngine;

public class Player : Entity
{
    [Header("Dash Inputs")]
    [HideInInspector]
    public Vector2 dashDirection;

    public float dashSpeed;
    public float dashDuration;
    private float normalDashSpeed;

    [Header("Movement Inputs")] public float moveSpeed = 10f;
    private float normalMoveSpeed;
   
    [Header("Equipment Tooltip")] public GameObject eKey;
    public GameObject equipmentInfo;
    public ItemTooltip itemTooltip;

    [Header("Hit Stop Effect")] [SerializeField]
    private float shakeForce;

    [SerializeField] private float changeTime;
    [SerializeField] private int restoreSpeed;
    [SerializeField] private float delay;

    public SkillManager OnSkill { get; private set; }
    public GameObject OnSword { get; private set; }

    public GameObject guardianAngel;

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
    public PlayerRessurrectState OnPlayerRessurrectState { get; private set; }

    #endregion

    [HideInInspector] public float lastImageXpos;
    [HideInInspector] public float lastImageYpos;
    
    public bool OnIsBusy { get; private set; }
    
    public PlayerInputs OnPlayerInputs { get; private set; }
    public UIInGame uiInGame;
    public UI uI;
    private HitStopEffect hitStopEffect;

    protected override void Awake()
    {
        base.Awake();
        OnPlayerInputs = new PlayerInputs();
        OnPlayerInputs.Player.Enable();
        OnStateMachine = new PlayerStateMachine();
        OnIdleState = new PlayerIdleState(this, OnStateMachine, "idle");
        OnMoveState = new PlayerMoveState(this, OnStateMachine, "walk");
        OnDashState = new PlayerDashState(this, OnStateMachine, "dash");
        OnPrimaryAttackState = new PlayerPrimaryAttackState(this, OnStateMachine, "attack");
        OnPlayerAimState = new PlayerAimState(this, OnStateMachine, "aimSword");
        OnPlayerCatchState = new PlayerCatchState(this, OnStateMachine, "catchSword");
        OnPlayerBlackholeState = new PlayerBlackholeState(this, OnStateMachine, "ultimate");
        OnDeathState = new PlayerDeathState(this, OnStateMachine, "death");
        OnPlayerRessurrectState = new PlayerRessurrectState(this, OnStateMachine, "resurrection");
    }

    protected override void Start()
    {
        base.Start();
        hitStopEffect = GetComponent<HitStopEffect>();

        OnSkill = SkillManager.Instance;
        OnStateMachine.Initialize(OnIdleState);

        normalMoveSpeed = moveSpeed;
        normalDashSpeed = dashSpeed;
    }

    protected override void Update()
    {
        if (Time.timeScale == 0 || DialogueManager.isActive)
        {
            SetZeroVelocity();
            OnStateMachine.ChangeState(OnIdleState);
            return;
        }
        
        base.Update();

        OnStateMachine.OnCurrentState.Update();

        CheckForDashInput();

        if (OnPlayerInputs.Player.Teleport.WasPressedThisFrame() && OnSkill.Kunai.KunaiUnlocked)
        {
            if (!OnSkill.Kunai.CanUseSkill())
            {
                SoundManager.Instance.PlaySoundEffects(32, null, false);
                OnEntityFx.CreateInformationText("In Cooldown");
            }
        }

        if (OnPlayerInputs.Player.Teleport.WasPressedThisFrame() && !OnSkill.Kunai.KunaiUnlocked)
        {
            SoundManager.Instance.PlaySoundEffects(32, null, false);
            OnEntityFx.CreateInformationText("Not Unlocked");
        }

        if (OnPlayerInputs.Player.UsePotion.WasPressedThisFrame())
        {
            Inventory.Instance.UsePotion();
        }

        if (Inventory.Instance.CheckForKey())
        {
            Inventory.Instance.keyCheck?.Invoke();
        }
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
        if (OnPlayerInputs.Player.Dash.WasPressedThisFrame() && SkillManager.Instance.Dash.CanUseSkill() &&
            OnSkill.Dash.DashUnlocked)
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
        else if (OnPlayerInputs.Player.Dash.WasPressedThisFrame() && (!SkillManager.Instance.Dash.CanUseSkill() || !OnSkill.Dash.DashUnlocked))
        {
            SoundManager.Instance.PlaySoundEffects(32, null, false);
            
            if (!OnSkill.Dash.DashUnlocked)
            {
                OnEntityFx.CreateInformationText("Not Unlocked");
            }
            else 
            {
                OnEntityFx.CreateInformationText("In Cooldown");
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

    public override void DamageEffect(GameObject sender)
    {
        if (OnEntityStats.isInvincible)
        {
            OnEntityFx.InvincibleEffect(2f);
            return;
        }

        base.DamageEffect(sender);
        hitStopEffect.Shake(sender.transform.position, shakeForce);
        hitStopEffect.StopTime(changeTime, restoreSpeed, delay);
    }

    protected override void ReturnToNormalSpeed()
    {
        base.ReturnToNormalSpeed();

        moveSpeed = normalMoveSpeed;
        dashSpeed = normalDashSpeed;
    }
}

