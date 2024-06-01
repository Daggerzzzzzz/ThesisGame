using UnityEngine;

public class VastoLordeIdleState : EnemyState
{
    protected EnemyVastoLorde enemy;

    public VastoLordeIdleState(Enemy enemyBase, EnemyStateMachine stateMachineState, string animationNameState, EnemyVastoLorde enemy) : base(enemyBase, stateMachineState, animationNameState)
    {
        this.enemy = enemy;
    }
    
    public override void Enter()
    {
        base.Enter();
        stateTimer = enemy.idleTime;
    }

    public override void Update()
    {
        base.Update();
        
        if (enemy.OnIsPlayerFollowing)
        {
            stateMachine.ChangeState(enemy.OnBattleState);
        }
        
        if(Input.GetKeyDown(KeyCode.V))
        {
            stateMachine.ChangeState(enemy.OnTeleportState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
