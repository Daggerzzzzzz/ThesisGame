public class EnemySkeleton : Enemy
{
    #region States
    public SkeletonIdleState OnIdleState { get; private set; }
    public SkeletonMoveState OnMoveState { get; private set; }
    public SkeletonBattleState OnBattleState { get; private set; }
    #endregion
    
    protected override void Awake()
    {
        base.Awake();
        OnIdleState = new SkeletonIdleState(this, OnStateMachine, "idle", this);
        OnMoveState = new SkeletonMoveState(this, OnStateMachine, "walk", this);
        OnBattleState = new SkeletonBattleState(this, OnStateMachine, "walk", this);
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
}
