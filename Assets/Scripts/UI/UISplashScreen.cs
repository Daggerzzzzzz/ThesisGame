using System.Collections;
using UnityEngine;

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
    [SerializeField]
    private UILoadingScreen uiLoadingScreen;

    private Coroutine introCoroutine;

    private void Start()
    {
        story1.SetActive(false);
        story2.SetActive(false);
        story3.SetActive(false);
        story4.SetActive(false);
        
        introCoroutine = StartCoroutine(WaitForIntro());
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (introCoroutine != null)
            {
                StopCoroutine(introCoroutine);
            }
            uiLoadingScreen.LoadLevel(2);
        }
    }

    private IEnumerator WaitForIntro()
    {
        story1.SetActive(true);
        Debug.Log("1ststory");
        yield return new WaitForSeconds(delayBetweenStories);

        story1.SetActive(false);
        story2.SetActive(true);
        Debug.Log("2ndstory");
        yield return new WaitForSeconds(delayBetweenStories);

        story2.SetActive(false);
        story3.SetActive(true);
        Debug.Log("3rdstory");
        yield return new WaitForSeconds(delayBetweenStories);

        story3.SetActive(false);
        story4.SetActive(true);
        Debug.Log("4thstory");
        yield return new WaitForSeconds(delayBetweenStories);

        uiLoadingScreen.LoadLevel(2);
    }
}