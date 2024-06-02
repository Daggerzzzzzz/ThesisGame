using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UISplashScreen : MonoBehaviour
{
    [SerializeField] 
    private GameObject story1;
    [SerializeField] 
    private GameObject story2;
    [SerializeField] 
    private GameObject story3;
    [SerializeField] 
    private GameObject story4;
    
    [SerializeField]
    private float delayBetweenStories = 2f;

    private void Start()
    {
        StartCoroutine(WaitForIntro());
    }

    private IEnumerator WaitForIntro()
    {
        story1.SetActive(false);
        story2.SetActive(false);
        story3.SetActive(false);
        story4.SetActive(false);

        story1.SetActive(true);
        yield return new WaitForSeconds(delayBetweenStories);

        story1.SetActive(false);
        story2.SetActive(true);
        yield return new WaitForSeconds(delayBetweenStories);

        story2.SetActive(false);
        story3.SetActive(true);
        yield return new WaitForSeconds(delayBetweenStories);

        story3.SetActive(false);
        story4.SetActive(true);
        yield return new WaitForSeconds(delayBetweenStories);

        SceneManager.LoadScene(2);
    }
}