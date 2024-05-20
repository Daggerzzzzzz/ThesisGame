using UnityEngine;

public class EnemyZombie : Enemy
{
    #region States
    public ZombieIdleState OnIdleState { get; private set; }
    public ZombieMoveState OnMoveState { get; private set; }
    public ZombieBattleState OnBattleState { get; private set; }
    public ZombieAttackState OnAttackState { get; private set; }
    public ZombieDeathState OnDeathState { get; private set; }

    #endregion
    
    protected override void Awake()
    {
        base.Awake();
        OnIdleState = new ZombieIdleState(this, OnStateMachine, "idle", this);
        OnMoveState = new ZombieMoveState(this, OnStateMachine, "walk", this);
        OnBattleState = new ZombieBattleState(this, OnStateMachine, "walk", this);
        OnAttackState = new ZombieAttackState(this, OnStateMachine, "attack", this);
        OnDeathState = new ZombieDeathState(this, OnStateMachine, "death", this);
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
