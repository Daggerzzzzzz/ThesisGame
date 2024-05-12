public class EnemyStateMachine 
{
   public EnemyState OnCurrentState { get; private set; }

   public void Initialize(EnemyState startState)
   {
      OnCurrentState = startState;
      OnCurrentState.Enter();
   }

   public void ChangeState(EnemyState newState)
   {
      OnCurrentState.Exit();
      OnCurrentState = newState;
      OnCurrentState.Enter();
   }
}
