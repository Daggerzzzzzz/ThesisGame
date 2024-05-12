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
   public bool KunaiDashUnlocked { get; private set; }

   protected override void Start()
   {
      base.Start();

      unlockDashButton.GetComponent<Button>().onClick.AddListener(UnlockDash);
      unlockCloneDashButton.GetComponent<Button>().onClick.AddListener(UnlockCloneDash);
      unlockKunaiDashButton.GetComponent<Button>().onClick.AddListener(UnlockKunaiDash);
      
      Debug.Log("DashSkill");
   }

   public override void UseSkill()
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
   
   private void UnlockKunaiDash()
   {
      if (unlockKunaiDashButton.unlocked)
      {
         KunaiDashUnlocked = true;
      }
   }

   public void CloneDash()
   {
      if (CloneDashUnlocked)
      {
         SkillManager.Instance.Clone.CreateClone(player.transform.position);
      }
   }

   public void KunaiDash()
   {
      if (KunaiDashUnlocked)
      {
         //Implement Kunai on Dash
      }
   }
}
