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
   
   protected PlayerInputs playerInputs;
   
   protected PlayerState(Player playerState, PlayerStateMachine stateMachineState, string animationNameState)
   {
      player = playerState;
      stateMachine = stateMachineState;
      animationName = animationNameState;
   }

   public virtual void Enter()
   {
      playerInputs = player.OnPlayerInputs;
      player.OnAnim.SetBool(animationName, true);
      rb = player.OnRb;
      triggerCalled = false;
   }

   public virtual void Update()
   {
      if (playerInputs.Player.Fire.IsPressed())
      {
         stateMachine.ChangeState(player.OnPrimaryAttackState);
      }

      if (playerInputs.Player.ThrowSword.WasPressedThisFrame() && HasNoSword() && player.OnSkill.Sword.swordFlyingUnlocked)
      {
         stateMachine.ChangeState(player.OnPlayerAimState);
      }

      if (playerInputs.Player.Ultimate.WasPressedThisFrame() && player.OnSkill.Blackhole.BaseUpgradeUnlock && player.OnSkill.Blackhole.CanUseSkill())
      {
         stateMachine.ChangeState(player.OnPlayerBlackholeState);
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
