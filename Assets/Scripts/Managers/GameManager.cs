using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingletonMonoBehavior<GameManager>
{
   public void RestartScene()
   {
      Scene gameScene = SceneManager.GetActiveScene();

      SceneManager.LoadScene(gameScene.name);
   }
}
