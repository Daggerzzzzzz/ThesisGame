using UnityEngine;

public class UIMainMenu : MonoBehaviour
{
   [SerializeField] 
   private GameObject creditScreen;
   [SerializeField] 
   private GameObject continueButton;

   private void Start()
   {
      if (SaveManager.Instance.HasSavedData() == false)
      {
         continueButton.SetActive(false);
      }
      
      SoundManager.Instance.PlayBackgroundMusic(4);
   }
   
   public void StartGame()
   {
      SaveManager.Instance.DeleteSavedData();
   }
   
   public void CreditsOpen()
   {
      creditScreen.SetActive(true);
   }
   
   public void CloseButton()
   {
      creditScreen.SetActive(false);
   }

   public void ExitGame()
   {
      Application.Quit();
   }

   public void PlayHoverSoundEffects()
   {
      SoundManager.Instance.PlaySoundEffects(23, null, false);
   }
   
   public void PlayClickSoundEffects()
   {
      SoundManager.Instance.StopSoundEffects(23);
      SoundManager.Instance.PlaySoundEffects(21, null, false);
   }
}
