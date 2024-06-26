using System.Collections;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField]
    private GameObject[] doors;
    [SerializeField]
    private GameObject mapHider;
    [SerializeField]
    private bool ifPressedOk;
    
    private GameObject uiGameObject;
    private UI ui;
    private BoxCollider2D boxCollider2D;

    public string roomCenterName;
    public bool roomActive;
    public bool closeWhenEntered;

    private bool startAlreadyOpened;
    
    private static readonly int Close = Animator.StringToHash("close");

    private void Start()
    {
        uiGameObject = GameObject.FindGameObjectWithTag("Pause");
        boxCollider2D = GetComponentInChildren<BoxCollider2D>();
        ui = uiGameObject.GetComponent<UI>();
        
        if (roomCenterName == "Room End" || roomCenterName == "End Center")
        {
            boxCollider2D.size = new Vector2(21.5f, 11.5f);
            boxCollider2D.offset = new Vector2(-1.5f, -0.5f);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision) 
    { 
        if ((roomCenterName == "Room Start" || roomCenterName == "Starting Center") & !startAlreadyOpened)
        {
            if (Inventory.Instance.armor.itemDataSo != null && Inventory.Instance.weapon.itemDataSo != null)
            {
                OpenDoors();
                startAlreadyOpened = true;
            }
        }
        
        if(collision.CompareTag("Player"))
        {
            if ((roomCenterName == "Room End" || roomCenterName == "End Center") && !ifPressedOk)
            {
                bool hasKey = Inventory.Instance.CheckForKey();

                if (hasKey)
                {
                    if (ui.aboutToEnterBossUI != null)
                    {
                        ui.SwitchMenus(ui.aboutToEnterBossUI);
                    }
                    ui.okBossWarningButton.onClick.AddListener(PressedOkInBossWarning);
                    ui.cancelBossWarningButton.onClick.AddListener(PressedCancelInBossWarning);
                }
                else if (!hasKey)
                {
                    if (ui.noKeyUI != null)
                    {
                        ui.SwitchMenus(ui.noKeyUI);
                    }
                    ui.okNoKeyButton.onClick.AddListener(PressedOkInNoKey);
                }
            }
            else if ((roomCenterName == "Room End" || roomCenterName == "End Center") && ifPressedOk)
            {
                SoundManager.Instance.PlaySoundEffects(0, null, false);
                StartCoroutine(BGMDelay());
                
                if(closeWhenEntered)
                {
                    Debug.Log("Closed When Entered");
                    CheckForKunaiExistence();
                    StartCoroutine(CloseDoorDelay());
                }

                roomActive = true; 
                mapHider.SetActive(false);
            
                CameraController.Instance.ChangeTarget(transform);
                CameraSwitch.Instance.PlayerEnter(); 
            }
            else
            {
                if(closeWhenEntered)
                {
                    Debug.Log("Closed When Entered");
                    CheckForKunaiExistence();
                    StartCoroutine(CloseDoorDelay());
                }

                roomActive = true; 
                mapHider.SetActive(false);
            
                CameraController.Instance.ChangeTarget(transform);
                CameraSwitch.Instance.PlayerEnter(); 
            }
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

    private IEnumerator BGMDelay()
    {
        yield return new WaitForSeconds(SoundManager.Instance.GetLengthSoundEffects(0));
        SoundManager.Instance.PlayBackgroundMusic(1);
    }

    public void OpenDoors()
    {
        SoundManager.Instance.PlaySoundEffects(3, gameObject.transform, true);
        Debug.Log("Open Doors");
        foreach(GameObject door in doors)
        {
            door.GetComponent<Animator>().SetBool(Close, false);
        }
    }
    
    private IEnumerator CloseDoorDelay()
    {
        yield return new WaitForSeconds(0.5f);
        startAlreadyOpened = false;
        StartCoroutine(CloseDoorDelaySounds());
        
        foreach(GameObject door in doors)
        {
            door.GetComponent<Animator>().SetBool(Close, true);
        }
    }
    
    private IEnumerator CloseDoorDelaySounds()
    {
        yield return new WaitForSeconds(0.75f);
        SoundManager.Instance.PlaySoundEffects(2, gameObject.transform, true);
    }

    private void CheckForKunaiExistence()
    {
        if (SkillManager.Instance.Kunai.CurrentKunai != null)
        {
            SkillManager.Instance.Kunai.currentKunaiSkillController.KunaiDestroy();
        }
    }

    public void CloseTheDoor()
    {
        StartCoroutine(CloseDoorDelay());
    }

    private void PressedOkInNoKey()
    {
        ui.SwitchMenus(ui.inGameUI);
        SoundManager.Instance.PlaySoundEffects(20, null, false);
    }
    
    private void PressedOkInBossWarning()
    {
        ui.SwitchMenus(ui.inGameUI);
        SoundManager.Instance.PlaySoundEffects(21, null, false);
        OpenDoors();
        ifPressedOk = true;
        boxCollider2D.size = new Vector2(19, 9.25f);
        boxCollider2D.offset = new Vector2(-1.6f, -0.5f);
    }
    
    private void PressedCancelInBossWarning()
    {
        ui.SwitchMenus(ui.inGameUI);
        SoundManager.Instance.PlaySoundEffects(20, null, false);
    }
}
