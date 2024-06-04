public class VastoLordeDeathState : EnemyState
{
    protected EnemyVastoLorde enemy;

    public VastoLordeDeathState(Enemy enemyBase, EnemyStateMachine stateMachineState, string animationNameState, EnemyVastoLorde enemy) : base(enemyBase, stateMachineState, animationNameState)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.NPC.SetActive(true);
        SoundManager.Instance.PlayBackgroundMusic(2);
    }

    public override void Update()
    {
        base.Update();
        enemy.SetZeroVelocity();
        enemy.OnCapsuleCollider2D.enabled = false;
    }

    public override void Exit()
    {
        base.Exit();
    }
}
