using UnityEngine;

public class VastoLordeSpellCastState : EnemyState
{
    protected EnemyVastoLorde enemy;
    
    private float spellTimer;
    private bool alreadySpawned;

    public VastoLordeSpellCastState(Enemy enemyBase, EnemyStateMachine stateMachineState, string animationNameState, EnemyVastoLorde enemy) : base(enemyBase, stateMachineState, animationNameState)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        spellTimer = 0.5f;
        stateTimer = 2;
    }

    public override void Update()
    {
        base.Update();
        spellTimer -= Time.deltaTime;

        enemy.SetZeroVelocity();
        
        if (spellTimer < 0 && !alreadySpawned)
        {
            enemy.CastCero();
            alreadySpawned = true;
        }
        
        if(stateTimer < 0)
        {
            stateMachine.ChangeState(enemy.OnTeleportState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        alreadySpawned = false;
    }
}
