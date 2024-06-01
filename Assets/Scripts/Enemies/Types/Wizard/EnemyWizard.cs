public class EnemyWizard : Enemy
{
    #region States
    public WizardIdleState OnIdleState { get; private set; }
    public WizardMoveState OnMoveState { get; private set; }
    public WizardBattleState OnBattleState { get; private set; }
    public WizardAttackState OnAttackState { get; private set; }
    public WizardDeathState OnDeathState { get; private set; }
    #endregion
    
    protected override void Awake()
    {
        base.Awake();
        OnIdleState = new WizardIdleState(this, OnStateMachine, "idle", this);
        OnMoveState = new WizardMoveState(this, OnStateMachine, "walk", this);
        OnBattleState = new WizardBattleState(this, OnStateMachine, "walk", this);
        OnAttackState = new WizardAttackState(this, OnStateMachine, "attack", this);
        OnDeathState = new WizardDeathState(this, OnStateMachine, "death", this);
    }
    
    protected override void Start()
    {
        base.Start();
        OnStateMachine.Initialize(OnIdleState);
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void EntityDeath()
    {
        base.EntityDeath();
        OnStateMachine.ChangeState(OnDeathState);
    }
}
