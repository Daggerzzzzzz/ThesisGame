using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIInGame : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] 
    private PlayerStats playerStats;
    [SerializeField] 
    private Player player;

    [Header("UI")]
    [SerializeField] 
    private GameObject healthUI;
    [SerializeField] 
    private Material material;
    
    [Header("Skills")]
    [SerializeField] 
    private Image dashImage;
    [SerializeField] 
    private Image kunaiImage;
    [SerializeField] 
    private Image swordImage;
    [SerializeField] 
    private Image ultimateImage;
    
    [Header("Inventory")]
    [SerializeField] 
    private Image potionImageCooldown;
    [SerializeField] 
    private Image potionImage;
    [SerializeField] 
    private Image armorImageCooldown;
    
    [Header("Warning UI")]
    public GameObject noKeyUI;
    public GameObject aboutToEnterBossUI;
    public Button okNoKeyButton;
    public Button okBossWarningButton;
    public Button cancelBossWarningButton;

    [SerializeField] private TextMeshProUGUI currentSouls;
    
    private SkillManager skill;
    
    private int potionCooldown;
    private int potionCount;

    private int armorCooldown;
    
    private static readonly int Health = Shader.PropertyToID("_Health");
    
    private void Awake()
    {
        material = healthUI.GetComponentInChildren<Image>().material;
    }

    private void Start()
    {
        if (playerStats != null)
        {
            UpdateHealthUI();
        }
        
        skill = SkillManager.Instance;
        
        if (potionCount <= 0)
        {
            potionImage.color = potionImageCooldown.color;
        }
        else if (potionCount > 0)
        {
            potionImage.color = Color.white;
        }
    }

    private void Update()
    {
        if (player.OnPlayerInputs.Player.Dash.WasPressedThisFrame() && skill.Dash.DashUnlocked)
        {
            SetCooldown(dashImage);
        }
        
        if (player.OnPlayerInputs.Player.Teleport.WasPressedThisFrame() && skill.Kunai.KunaiUnlocked)
        {
            SetCooldown(kunaiImage);
        }
        
        if (player.OnPlayerInputs.Player.ThrowSword.WasPressedThisFrame() && skill.Sword.swordFlyingUnlocked)
        {
            SetCooldown(swordImage);
        }
        
        if (player.OnPlayerInputs.Player.Ultimate.WasPressedThisFrame() && skill.Blackhole.BaseUpgradeUnlock)
        {
            SetCooldown(ultimateImage);
        }

        if (potionCount <= 0)
        {
            potionImage.color = potionImageCooldown.color;
        }
        else if (potionCount > 0)
        {
            potionImage.color = Color.white;
        }
       
        CheckCooldown(dashImage, skill.Dash.cooldown);
        CheckCooldown(kunaiImage, skill.Kunai.cooldown);
        CheckCooldown(swordImage, skill.Sword.cooldown);
        CheckCooldown(ultimateImage, skill.Blackhole.cooldown);
        CheckCooldown(potionImageCooldown, potionCooldown);
        CheckCooldown(armorImageCooldown, armorCooldown);

        currentSouls.text = PlayerManager.Instance.CurrentSouls().ToString();
    }
    
    public void UpdateHealthUI()
    {
        int maxHealth = playerStats.CalculateMaxHealthValue();
        int currentHealth = playerStats.currentHealth;
        
        float normalizedHealth = (float)currentHealth / maxHealth;
        
        material.SetFloat(Health, normalizedHealth);
    }

    private void SetCooldown(Image image)
    {
        if (image.fillAmount <= 0)
        {
            image.fillAmount = 1;
        }
    }

    private void CheckCooldown(Image image, float cooldown)
    {
        if (image.fillAmount > 0)
        {
            image.fillAmount -= 1 / cooldown * Time.deltaTime;
        }
    }

    public void CheckForPotionInput(int cooldown)
    {
        potionCooldown = cooldown;
        if (potionCount > 1)
        {
            SetCooldown(potionImageCooldown);
        }
    }
    
    public void NumberOfPotion(int stack)
    {
        potionCount = stack;
    }

    public void CheckForArmorInput(int cooldown)
    {
        armorCooldown = cooldown;
        SetCooldown(armorImageCooldown);
    }
}
