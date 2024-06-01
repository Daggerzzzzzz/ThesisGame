using UnityEngine;

public class PlayerRessurrectState : PlayerState
{
    public PlayerRessurrectState(Player playerState, PlayerStateMachine stateMachineState, string animationNameState) : base(playerState, stateMachineState, animationNameState)
    {
        
    }

    public override void Enter()
    {
        base.Enter();
        
        player.OnEntityStats.MakeInvincible(10f);
        
        SoundManager.Instance.PlaySoundEffects(11, null, false);
        exitResurrectionTimer = exitRessurectionTimerDelay;
        player.guardianAngel.SetActive(true);
    }

    public override void Update()
    {
        base.Update();
        
        if (exitResurrectionTimer > 0)
        {
            exitResurrectionTimer -= Time.deltaTime;
            if (exitResurrectionTimer <= 0)
            {
                player.OnStateMachine.ChangeState(player.OnIdleState);
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
        SoundManager.Instance.StopSoundEffects(11);
        player.guardianAngel.SetActive(false);
        player.OnCapsuleCollider2D.enabled = true;
        player.OnBoxCollider2D.enabled = true;
    }
}
