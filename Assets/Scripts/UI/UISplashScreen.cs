using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UISplashScreen : MonoBehaviour
{
    private string movie = "Assets/Sprites/UI/LastUI/Figsa.mp4";

    private void Start()
    {
        StartCoroutine(WaitForIntro(movie));
    }

    private IEnumerator WaitForIntro(string video)
    {
        Handheld.PlayFullScreenMovie(video, Color.black, FullScreenMovieControlMode.Hidden,
            FullScreenMovieScalingMode.Fill);
        yield return new WaitForEndOfFrame();
        SceneManager.LoadScene(1);
    }
}
