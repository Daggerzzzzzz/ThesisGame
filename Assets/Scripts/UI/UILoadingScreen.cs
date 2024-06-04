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
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(loadLevel);

        while (!loadOperation.isDone)
        {
            yield return null;
        }
    }
}
