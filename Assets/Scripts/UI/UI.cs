using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    [Header("End Screen")]
    [SerializeField]
    private UiFadeScreen fadeScreen;
    [SerializeField]
    private GameObject endText;
    [SerializeField] 
    private GameObject restartButton;
    [SerializeField] 
    private GameObject mainMenuButton;
    [Space]
    
    [Header("Navigation")]
    [SerializeField]
    private GameObject characterUI;
    [SerializeField] 
    private GameObject skillTreeUI;
    [SerializeField] 
    private GameObject settingsUI;
    [SerializeField] 
    private GameObject inGameUI;
    [SerializeField] 
    private Player player;
    
    [Header("Stats Button")]
    [SerializeField]
    private GameObject strengthButton;
    [SerializeField] 
    private GameObject agilityButton;
    [SerializeField] 
    private GameObject vitalityButton;
    [SerializeField] 
    private GameObject intelligenceButton;
    
    public ItemTooltip itemTooltip;
    public StatTooltip statTooltip;
    public SkillTooltip skillTooltip;

    private void Awake()
    {
        SwitchMenus(skillTreeUI);
    }

    private void Start()
    {
        SwitchMenus(inGameUI);
        DisableTooltips();
    }
    
    private void Update()
    {
        if (player == null)
        {
            return;
        }

        SetUpStatsButton(PlayerManager.Instance.statPoints != 0);

        if (player.OnPlayerInputs.Player.CharacterPanel.WasPressedThisFrame())
        {
            SwitchMenusWithKeyboard(characterUI);
        }
        
        if (player.OnPlayerInputs.Player.SkillTreePanel.WasPressedThisFrame())
        {
            SwitchMenusWithKeyboard(skillTreeUI);
        }
        
        if (player.OnPlayerInputs.Player.SettingsPanel.WasPressedThisFrame())
        {
            SwitchMenusWithKeyboard(settingsUI);
        }
    }

    public void SwitchMenus(GameObject menu)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            bool isFadingScreen = transform.GetChild(i).GetComponent<UiFadeScreen>() != null;
            if (!isFadingScreen)
            { 
                transform.GetChild(i).gameObject.SetActive(false);
                DisableTooltips();
            }
        }   

        if (menu != null)
        {
            menu.SetActive(true);
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
        StartCoroutine(LoadSceneWithEffect(1f));
    }

    public void EnableEndScreen()
    {
        SwitchMenus(null);
        fadeScreen.FadeOut();
        StartCoroutine(EndScreenDelay());
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

    private IEnumerator EndScreenDelay()
    {
        yield return new WaitForSeconds(1);
        endText.SetActive(true);
        yield return new WaitForSeconds(1);
        restartButton.SetActive(true);
        mainMenuButton.SetActive(true);
    }
    
    private IEnumerator LoadSceneWithEffect(float delay)
    {
        fadeScreen.FadeOut();
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(0);
    }
}
