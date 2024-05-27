using UnityEngine;
using UnityEngine.UI;

public class DashSkill : Skill
{
   [Header("Dash")] 
   [SerializeField] 
   private SkillTreeSlot unlockDashButton;
   public bool DashUnlocked { get; private set; }
      
   [Header("Summon Clone On Dash")] 
   [SerializeField] 
   private SkillTreeSlot unlockCloneDashButton;
   public bool CloneDashUnlocked { get; private set; }
   
   [Header("Summon Kunai On Dash")] 
   [SerializeField] 
   private SkillTreeSlot unlockKunaiDashButton;
   public bool DoubleCloneDashUnlocked { get; private set; }

   protected override void Start()
   {
      base.Start();

      unlockDashButton.GetComponent<Button>().onClick.AddListener(UnlockDash);
      unlockCloneDashButton.GetComponent<Button>().onClick.AddListener(UnlockCloneDash);
      unlockKunaiDashButton.GetComponent<Button>().onClick.AddListener(UnlockDoubleCloneDash);
   }

   protected override void UseSkill()
   {
      base.UseSkill();
   }

   private void UnlockDash()
   {
      if (unlockDashButton.unlocked)
      {
         DashUnlocked = true;
      }
   }
   
   private void UnlockCloneDash()
   {
      if (unlockCloneDashButton.unlocked)
      {
         CloneDashUnlocked = true;
      }
   }
   
   private void UnlockDoubleCloneDash()
   {
      if (unlockKunaiDashButton.unlocked)
      {
         DoubleCloneDashUnlocked = true;
      }
   }

   public void CloneDash()
   {
      if (CloneDashUnlocked)
      {
         EquipmentDataSO currentWeapon;
         if (Inventory.Instance.GetEquipment(EquipmentType.WEAPON) != null)
         {
            currentWeapon = Inventory.Instance.GetEquipment(EquipmentType.WEAPON);
            if (currentWeapon.itemName == "Zangetsu")
            {
               Inventory.Instance.GetEquipment(EquipmentType.WEAPON).UseEffect(player.dashDirection, player.GetComponent<PlayerStats>());
            }
         }
         else
         {
            SkillManager.Instance.Clone.CreateClone(player.transform.position, player.dashDirection, player.OnAttackDirection);
         }
      }
   }

   public void DoubleCloneDash()
   {
      if (DoubleCloneDashUnlocked)
      {
         EquipmentDataSO currentWeapon;
         currentWeapon = Inventory.Instance.GetEquipment(EquipmentType.WEAPON);
         if (currentWeapon.itemName == "Zangetsu")
         {
            Inventory.Instance.GetEquipment(EquipmentType.WEAPON).UseEffect(-player.dashDirection, player.GetComponent<PlayerStats>());
         }
         else
         {
            SkillManager.Instance.Clone.CreateClone(player.transform.position, -player.dashDirection, -player.OnAttackDirection);
         }
      }
   }

   protected override void CheckUnlocked()
   {
      UnlockDash();
      UnlockCloneDash();
      UnlockDoubleCloneDash();
   }
}
