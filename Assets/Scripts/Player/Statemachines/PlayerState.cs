using UnityEngine;

public class PlayerState
{
   protected PlayerStateMachine stateMachine;
   protected Player player;
   protected Rigidbody2D rb;

   private string animationName;
   private float xInput;
   private float yInput;
   
   protected Vector2 movementInput;
   protected float stateTimer;
   protected bool triggerCalled;
   protected float distanceBetweenImages = 0.025f;
   
   protected float resurrectionTimer;
   protected float resurrectionDelay = 1f;

   protected float exitResurrectionTimer;
   protected float exitRessurectionTimerDelay = 1f;
   
   protected PlayerState(Player playerState, PlayerStateMachine stateMachineState, string animationNameState)
   {
      player = playerState;
      stateMachine = stateMachineState;
      animationName = animationNameState;
   }

   public virtual void Enter()
   {
      player.OnAnim.SetBool(animationName, true);
      rb = player.OnRb;
      triggerCalled = false;
   }

   public virtual void Update()
   {
      if ( player.OnPlayerInputs.Player.Fire.IsPressed())
      {
         stateMachine.ChangeState(player.OnPrimaryAttackState);
      }

      if ( player.OnPlayerInputs.Player.ThrowSword.WasPressedThisFrame() && HasNoSword() && player.OnSkill.Sword.swordFlyingUnlocked)
      {
         stateMachine.ChangeState(player.OnPlayerAimState);
      }
      else if ( player.OnPlayerInputs.Player.ThrowSword.WasPressedThisFrame() && HasNoSword() && !player.OnSkill.Sword.swordFlyingUnlocked)
      {
         SoundManager.Instance.PlaySoundEffects(32, null, false);
         player.OnEntityFx.CreateInformationText("Not Unlocked");
      }

      if ( player.OnPlayerInputs.Player.Ultimate.WasPressedThisFrame() && player.OnSkill.Blackhole.BaseUpgradeUnlock && player.OnSkill.Blackhole.CanUseSkill())
      {
         stateMachine.ChangeState(player.OnPlayerBlackholeState);
      }
      else if ( player.OnPlayerInputs.Player.Ultimate.WasPressedThisFrame() && (!player.OnSkill.Blackhole.BaseUpgradeUnlock || !player.OnSkill.Blackhole.CanUseSkill()))
      {
         SoundManager.Instance.PlaySoundEffects(32, null, false);

         if (!player.OnSkill.Blackhole.BaseUpgradeUnlock)
         {
            player.OnEntityFx.CreateInformationText("Not Unlocked");
         }
         else 
         {
            player.OnEntityFx.CreateInformationText("In Cooldown");
         }
      }
      
      stateTimer -= Time.deltaTime;
      xInput = Input.GetAxisRaw("Horizontal");
      yInput = Input.GetAxisRaw("Vertical");
      movementInput = new Vector2(xInput, yInput).normalized;
   }

   public virtual void Exit()
   {
      player.OnAnim.SetBool(animationName, false);
   }

   public virtual void PlayerAnimationFinishTrigger()
   {
      triggerCalled = true;
   }

   private bool HasNoSword()
   {
      if (!player.OnSword)
      {
         return true;
      }

      player.OnSword.GetComponent<SwordSkillController>().ReturnSword();
      return false;
   }
}
