using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMainMenu : MonoBehaviour
{
   [SerializeField] 
   private int sceneGameNum = 1;

   public void StartGame()
   {
      SceneManager.LoadScene(sceneGameNum);
   }

   public void ExitGame()
   {
      Debug.Log("Exit Game");
      //Application.Quit();
   }
}
