using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerManager : SingletonMonoBehavior<PlayerManager>, ISaveManager
{
   [Header("Player Info")]
   public Player player;
   public int soulCurrency;
   public int statPoints;
   
   [Header("Player Level")]
   public int level;
   public float currentExp;
   public float requiredExp;

   [Header("UI")] 
   public Image frontExpBar;
   public Image backExpBar;
   public float timerDelay;
   public TextMeshProUGUI levelText;
   public TextMeshProUGUI statText;
   
   [Header("Multipliers")] 
   [Range(1f, 300f)]
   public float additionMultiplier;
   [Range(2f, 4)]
   public float powerMultiplier;
   [Range(7f, 14f)]
   public float divisionMultiplier;

   private float lerpTimer;
   private float delayTimer;

   private void Start()
   {
      frontExpBar.fillAmount = currentExp / requiredExp;
      backExpBar.fillAmount = currentExp / requiredExp;
      requiredExp = CalculateRequiredExperience();
      levelText.text = "LVL." + level;
      statText.text = "POINTS: " + statPoints;
   }

   private void Update()
   {
      UpdateExpUI();
      
      if (currentExp > requiredExp)
      {
         LevelUp();
      }
   }

   public bool HaveEnoughCurrency(int price)
   {
      if (price > soulCurrency)
      {
         Debug.Log("not enough money");
         return false;
      }

      soulCurrency -= price;

      return true;
   }
   
   public int CurrentSouls()
   {
      return soulCurrency;
   }

   private void UpdateExpUI()
   {
      float expFraction = currentExp / requiredExp;
      float fillExp = frontExpBar.fillAmount;

      if (fillExp < expFraction)
      {
         delayTimer += Time.deltaTime;
         backExpBar.fillAmount = expFraction;

         if (delayTimer > timerDelay)
         {
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / 4;
            frontExpBar.fillAmount = Mathf.Lerp(fillExp, backExpBar.fillAmount, percentComplete);
         }
      }
   }

   public void GainExperienceFlatRate(float expGained)
   {
      currentExp += expGained;
      lerpTimer = 0f;
   }

   private void LevelUp()
   {
      level++;
      statPoints++;
      frontExpBar.fillAmount = 0f;
      backExpBar.fillAmount = 0f;
      currentExp = Mathf.RoundToInt(currentExp - requiredExp);
      requiredExp = CalculateRequiredExperience();
      levelText.text = "LVL." + level;
      statText.text = "POINTS: " + statPoints;
   }

   private int CalculateRequiredExperience()
   {
      int solveForRequiredExp = 0;

      for (int levelCycle = 1; levelCycle <= level; levelCycle++)
      {
         solveForRequiredExp += (int)Mathf.Floor(levelCycle + additionMultiplier * Mathf.Pow(powerMultiplier, levelCycle / divisionMultiplier));
      }

      return solveForRequiredExp / 4;
   }

   public void LoadData(GameData data)
   {
      soulCurrency = data.soulCurrency;
      statPoints = data.statPoints;
      level = data.level;
      currentExp = data.currentExp;
   }

   public void SaveData(ref GameData data)
   {
      data.soulCurrency = soulCurrency;
      data.statPoints = statPoints;
      data.level = level;
      data.currentExp = currentExp;
   }

   public void UpdateStatPointsUI()
   {
      statText.text = "POINTS: " + statPoints;
   }
}
