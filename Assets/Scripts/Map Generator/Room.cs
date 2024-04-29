using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField]
    public bool closeWhenEntered;
    [SerializeField]
    private GameObject[] doors;
    
    public bool roomActive;
    
    private static readonly int Close = Animator.StringToHash("close");

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if(collision.CompareTag("Player"))
        {
            if(closeWhenEntered)
            {
                StartCoroutine(DoorDelay());
            }

            roomActive = true; 
            CameraController.Instance.ChangeTarget(transform);
            CameraSwitch.Instance.PlayerEnter(); 
        }
    }
    
    private void OnTriggerExit2D(Collider2D collision) 
    {
        if(collision.CompareTag("Player"))
        {
            CameraSwitch.Instance.PlayerExit();
            roomActive = false;
        }
    }

    public void OpenDoors()
    {
        foreach(GameObject door in doors)
        {
            door.GetComponent<Animator>().SetBool(Close, false);
            closeWhenEntered = false;
        } 
    }
    
    private IEnumerator DoorDelay()
    {
        yield return new WaitForSeconds(0.5f);
        CheckForKunaiExistence();
        foreach(GameObject door in doors)
        {
            door.GetComponent<Animator>().SetBool(Close, true);
        }
    }

    private void CheckForKunaiExistence()
    {
        if (SkillManager.Instance.Kunai.CurrentKunai != null)
        {
            SkillManager.Instance.Kunai.currentKunaiSkillController.KunaiDestroy();
        }
    }
}
