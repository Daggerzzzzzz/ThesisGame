using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField]
    private GameObject portal;
    
    public DialogueTrigger dialogueTrigger;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            dialogueTrigger.StartDialogue();
            if (portal != null)
            {
                portal.SetActive(true);
            }
        }
    }
}
