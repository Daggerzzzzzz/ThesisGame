using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour, ISaveManager
{
    [Header("End Screen")]
    public TextMeshProUGUI text;
    public GameObject endText;
    [Space]
    
    [Header("Navigation")]
    [SerializeField]
    private GameObject characterUI;
    [SerializeField] 
    private GameObject skillTreeUI;
    [SerializeField] 
    private GameObject settingsUI;
    [SerializeField] 
    private GameObject dialogueUI;
    [SerializeField] 
    private Player player;
    public GameObject inGameUI;
    public GameObject gameOver;
    
    [Header("Stats Button")]
    [SerializeField]
    private GameObject strengthButton;
    [SerializeField] 
    private GameObject agilityButton;
    [SerializeField] 
    private GameObject vitalityButton;
    [SerializeField] 
    private GameObject intelligenceButton;
    
    [Header("Tooltips")]
    public ItemTooltip itemTooltip;
    public StatTooltip statTooltip;
    public SkillTooltip skillTooltip;

    [Header("Sliders")] 
    [SerializeField] 
    private UIVolumeSlider[] volumeSettings;
    
    [Header("Warning UI")]
    public GameObject noKeyUI;
    public GameObject aboutToEnterBossUI;
    public Button okNoKeyButton;
    public Button okBossWarningButton;
    public Button cancelBossWarningButton;

    private void Awake()
    {
        SwitchMenus(skillTreeUI);
    }

    private void Start()
    {
        text = endText.GetComponent<TextMeshProUGUI>();
        text.text = "YOU DIED";
        
        SwitchMenus(inGameUI);
        DisableTooltips();
    }
    
    private void Update()
    {
        SetUpStatsButton(PlayerManager.Instance.statPoints != 0);
            
        if (player == null)
        {
            return;
        }

        if (DialogueManager.isActive)
        {
            SwitchMenus(dialogueUI);
        }
        else if(!DialogueManager.isActive)
        {
            dialogueUI.SetActive(false);
            CheckInGameUI();
        }

        if (player.OnPlayerInputs.Player.CharacterPanel.WasPressedThisFrame())
        {
            SoundManager.Instance.StopSoundEffects(23);
            SoundManager.Instance.PlaySoundEffects(23, null, false);
            SwitchMenusWithKeyboard(characterUI);
        }
        
        if (player.OnPlayerInputs.Player.SkillTreePanel.WasPressedThisFrame())
        {
            SoundManager.Instance.StopSoundEffects(23);
            SoundManager.Instance.PlaySoundEffects(23, null, false);
            SwitchMenusWithKeyboard(skillTreeUI);
        }
        
        if (player.OnPlayerInputs.Player.SettingsPanel.WasPressedThisFrame())
        {
            SoundManager.Instance.StopSoundEffects(23);
            SoundManager.Instance.PlaySoundEffects(23, null, false);
            SwitchMenusWithKeyboard(settingsUI);
        }
    }

    public void SwitchMenus(GameObject menu)
    
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
            DisableTooltips();
        }   

        if (menu != null)
        {
            menu.SetActive(true);
        }
        
        ControlCursor(menu);
        
        Debug.Log(menu);

        if (GameManager.Instance != null)
        {
            if (menu == inGameUI || menu == dialogueUI)
            {
                GameManager.Instance.PauseGame(false);
            }
            else 
            {
                GameManager.Instance.PauseGame(true);
            }
        }
    }

    public void SwitchMenusWithKeyboard(GameObject menu)
    {
        if (menu != null && menu.activeSelf)
        {
            menu.SetActive(false);
            CheckInGameUI();
            DisableTooltips();
            return;
        }

        SwitchMenus(menu);
    }

    private void CheckInGameUI()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeSelf)
            {
                return;
            }
        }

        SwitchMenus(inGameUI);
    }

    public void RestartGameButton()
    {
        GameManager.Instance.RestartScene();
    }
    
    public void ReturnToMainMenu()
    {
        GameManager.Instance.ReturnToMainMenu();
    }
    
    private void DisableTooltips()
    {
        itemTooltip.gameObject.SetActive(false);
        statTooltip.gameObject.SetActive(false);
        skillTooltip.gameObject.SetActive(false);
    }

    private void SetUpStatsButton(bool check)
    {
        strengthButton.SetActive(check);
        agilityButton.SetActive(check);
        intelligenceButton.SetActive(check);
        vitalityButton.SetActive(check);
    }
    
    public void LoadData(GameData data)
    {
        foreach (KeyValuePair <string, float> pair in data.volumeSettings)
        {
            foreach (UIVolumeSlider item in volumeSettings)
            {
                if (item.parameter == pair.Key)
                {
                    item.LoadSlider(pair.Value);
                }
            }
        }
    }

    public void SaveData(ref GameData data)
    {
        data.volumeSettings.Clear();

        foreach (UIVolumeSlider item in volumeSettings)
        {
            data.volumeSettings.Add(item.parameter, item.slider.value);
        }
    }

    public void HoverEnterSound()
    {
        SoundManager.Instance.PlaySoundEffects(23, null, false);
    }
    
    public void PlayClickSoundEffects()
    {
        SoundManager.Instance.StopSoundEffects(23);
        SoundManager.Instance.PlaySoundEffects(21, null, false);
    }
    
    private void ControlCursor(GameObject menu)
    {
        if (menu == inGameUI || menu == dialogueUI)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else 
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
