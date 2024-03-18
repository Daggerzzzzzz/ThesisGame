using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState
{
   protected EnemyStateMachine stateMachine;
   protected Enemy enemy;

   protected bool triggerCalled;
   protected float stateTimer;
   protected Vector2 attackDirection;
   protected Vector2 enemyMovementInput;
   private string animationName;

   public EnemyState(Enemy enemyBase, EnemyStateMachine stateMachineState, string animationNameState)
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
   
   protected void AttackDirection()
   {
      if (Mathf.Abs(enemyMovementInput.x) > Mathf.Abs(enemyMovementInput.y))
      {
         if (enemyMovementInput.x > 0)
         {
            attackDirection = new Vector2(0.15f, 0f);
         }
         if (enemyMovementInput.x < 0)
         {
            attackDirection = new Vector2(-0.15f, 0f);
         }
      }
      if (Mathf.Abs(enemyMovementInput.x) < Mathf.Abs(enemyMovementInput.y))
      {
         if (enemyMovementInput.y > 0)
         {
            attackDirection = new Vector2(0f, 0.14f);
         }
         if (enemyMovementInput.y < 0)
         {
            attackDirection = new Vector2(0f, -0.14f);
         }
      }
   }
}
