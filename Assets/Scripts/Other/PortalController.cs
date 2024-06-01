using System.Collections;
using UnityEngine;

public class PortalController : MonoBehaviour
{
    [SerializeField] 
    private UI ui;
    [SerializeField] 
    private GameObject winScreen;

    private Animator anim;
    private static readonly int Exit = Animator.StringToHash("exit");

    private void Start()
    {
        ui = GameObject.FindGameObjectWithTag("Pause").GetComponent<UI>();
        anim = GetComponent<Animator>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ui.text.text = "YOU WIN";
            anim.SetTrigger(Exit);
            StartCoroutine(WinScreenDelay());
        }
    }

    private IEnumerator WinScreenDelay()
    {
        ui.fadeScreen.FadeOut();
        yield return new WaitForSeconds(1f);
        ui.endText.SetActive(true);
        yield return new WaitForSeconds(1f);
        ui.restartButton.SetActive(true);
        ui.mainMenuButton.SetActive(true);
        GameManager.Instance.PauseGame(true);
        SaveManager.Instance.DeleteSavedData();
    }
}
