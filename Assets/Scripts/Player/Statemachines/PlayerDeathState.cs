using UnityEngine;

public class PlayerDeathState : PlayerState
{
    public PlayerDeathState(Player playerState, PlayerStateMachine stateMachineState, string animationNameState) : base(playerState, stateMachineState, animationNameState)
    {
        
    }
    
    public override void Enter()
    {
        base.Enter();
        
        player.OnPlayerInputs.Player.Disable();
        
        EquipmentDataSO currentArmor = Inventory.Instance.GetEquipment(EquipmentType.ARMOR);
        
        if (currentArmor.itemName == "Guardian Angel")
        {
            Inventory.Instance.GetEquipment(EquipmentType.ARMOR).UseEffect(player.transform.position, player.OnEntityStats);
            
            player.OnEntityStats.isDead = false;
            resurrectionTimer = resurrectionDelay;
        }
    }

    public override void Update()
    {
        base.Update();
        
        if (player.OnEntityStats.currentHealth > 0)
        { 
            if (resurrectionTimer > 0)
            {
                resurrectionTimer -= Time.deltaTime;
                if (resurrectionTimer <= 0)
                {
                    ResurrectionDelay();
                }
            }
        }
        else if (player.OnEntityStats.currentHealth < 0)
        {
            player.uI.youWin.SetActive(false);
            player.uI.youWin.SetActive(false);
            player.uI.youLose.SetActive(true);
            player.uI.EndScreen();
            if (!soundAlreadyPlayed)
            {
                SoundManager.Instance.PlaySoundEffects(33, null, false);
                soundAlreadyPlayed = true;
            }
        }
        
        player.SetZeroVelocity();
        player.OnCapsuleCollider2D.enabled = false;
        player.OnBoxCollider2D.enabled = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void PlayerAnimationFinishTrigger()
    {
        base.PlayerAnimationFinishTrigger();
    }

    private void ResurrectionDelay()
    { 
        player.OnStateMachine.ChangeState(player.OnPlayerRessurrectState);
    }
}
