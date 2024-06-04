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
            ui.youWin.SetActive(true);
            ui.youLose.SetActive(false);
            anim.SetTrigger(Exit);
            ui.EndScreen();
        }
    }
}
