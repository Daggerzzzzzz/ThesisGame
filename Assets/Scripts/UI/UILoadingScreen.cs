using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UILoadingScreen : MonoBehaviour
{
    [SerializeField] 
    private GameObject loadingScreen;
    [SerializeField] 
    private GameObject mainMenu;
    
    public void LoadLevel(int loadLevel)
    {
        mainMenu.SetActive(false);
        loadingScreen.SetActive(true);
        
        StartCoroutine(LoadLevelAsync(loadLevel));
    }

    private IEnumerator LoadLevelAsync(int loadLevel)
    { 
        yield return new WaitForSeconds(1f);
        
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(loadLevel);

        while (!loadOperation.isDone)
        {
            yield return null;
        }
        
        loadingScreen.SetActive(false);
    }
}
