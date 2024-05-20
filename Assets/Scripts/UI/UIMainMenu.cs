using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMainMenu : MonoBehaviour
{
   [SerializeField] 
   private int sceneGameNum = 1;
   [SerializeField] 
   private UiFadeScreen fadeScreen;
   [SerializeField] 
   private GameObject continueButton;

   private void Start()
   {
      if (SaveManager.Instance.HasSavedData() == false)
      {
         continueButton.SetActive(false);
      }
   }

   public void ContinueGame()
   {
      StartCoroutine(LoadSceneWithEffect(1f));
   }
   
   public void StartGame()
   {
      SaveManager.Instance.DeleteSavedData();
      StartCoroutine(LoadSceneWithEffect(1f));
   }

   public void ExitGame()
   {
      Debug.Log("Exit Game");
      //Application.Quit();
   }

   private IEnumerator LoadSceneWithEffect(float delay)
   {
      fadeScreen.FadeOut();
      yield return new WaitForSeconds(delay);
      SceneManager.LoadScene(sceneGameNum);
   }
}
