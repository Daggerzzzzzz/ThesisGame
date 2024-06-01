using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UISplashScreen : MonoBehaviour
{
    [SerializeField] 
    private float waitTime;

    private void Start()
    {
        StartCoroutine(WaitForIntro());
    }

    private IEnumerator WaitForIntro()
    {
        yield return new WaitForSeconds(waitTime);

        SceneManager.LoadScene(1);
    }
}
