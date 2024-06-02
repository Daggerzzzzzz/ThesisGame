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
        
        if (player.OnEntityStats.currentHealth < 0)
        { 
            GameObject.Find("Pause").GetComponent<UI>().EnableEndScreen();
            SoundManager.Instance.PlaySoundEffects(33, null, false);
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
