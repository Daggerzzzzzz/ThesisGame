using UnityEngine;

public class GameManager : SingletonMonoBehavior<GameManager>
{
   [SerializeField] 
   private UILoadingScreen uiLoadingScreen;
   
   public void RestartScene()
   {
      uiLoadingScreen.LoadLevel(2);
   }
   
   public void ReturnToMainMenu()
   {
      uiLoadingScreen.LoadLevel(0);
   }
   
   public void PauseGame(bool pause)
   {
      if (pause)
      {
         Time.timeScale = 0;
      }
      else
      {
         Time.timeScale = 1;
      }
   }
}
