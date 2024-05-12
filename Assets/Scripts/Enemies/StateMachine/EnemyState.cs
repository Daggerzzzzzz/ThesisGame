using UnityEngine;

public class EnemyState
{
   protected EnemyStateMachine stateMachine;
   private Enemy enemy;

   protected bool triggerCalled;
   protected float stateTimer;
   private string animationName;

   protected EnemyState(Enemy enemyBase, EnemyStateMachine stateMachineState, string animationNameState)
   {
      enemy = enemyBase;
      stateMachine = stateMachineState;
      animationName = animationNameState;
   }

   public virtual void Enter()
   {
      triggerCalled = false;
      enemy.OnAnim.SetBool(animationName, true);
   }

   public virtual void Update()
   {
      stateTimer -= Time.deltaTime;
   }

   public virtual void Exit()
   {
      enemy.OnAnim.SetBool(animationName, false);
   }
   
   public virtual void EnemyAnimationFinishTrigger()
   {
      triggerCalled = true;
   }
}
