using UnityEngine;

public class PlayerTrigger : MonoBehaviour
{
    private Player OnPlayer => GetComponentInParent<Player>();
    
    private bool attackOnce;
    
    private void PlayerAnimation()
    {
        OnPlayer.AnimationTriggerForPlayer();
        ResetAttack();
    }

    private void AttackTrigger()
    {
        SoundManager.Instance.PlaySoundEffects(7, null, true);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(OnPlayer.attackCheck.position, OnPlayer.attackCheckRadius);
        EquipmentDataSO currentWeapon;
        EquipmentDataSO currentArmor;
        
        foreach (var hit in colliders)
        {
            if (hit.CompareTag("Enemy Trigger Collider") && !attackOnce)
            {
                SoundManager.Instance.PlaySoundEffects(8, null, true);
                EnemyStats target = hit.GetComponentInParent<EnemyStats>();
                
                OnPlayer.OnEntityStats.DoDamage(target,gameObject);
                
                currentWeapon = Inventory.Instance.GetEquipment(EquipmentType.WEAPON);
                currentArmor = Inventory.Instance.GetEquipment(EquipmentType.ARMOR);
                
                if (currentWeapon.itemName == "Gonryomaru")
                {
                    Inventory.Instance.GetEquipment(EquipmentType.WEAPON).UseEffect(target.transform.position, OnPlayer.GetComponent<PlayerStats>());
                }
                else if (currentWeapon.itemName == "Ryujin Jakka")
                {
                    Inventory.Instance.GetEquipment(EquipmentType.WEAPON).UseEffect(target.transform.position, OnPlayer.GetComponent<PlayerStats>());
                }
                
                if (currentArmor.itemName == "Warmogs")
                {
                    Inventory.Instance.GetEquipment(EquipmentType.ARMOR).UseEffect(target.transform.position, OnPlayer.GetComponent<PlayerStats>());
                }
                
                attackOnce = true;
            }
        }
    }
    
    private void ResetAttack()
    {
        attackOnce = false;
    }

    private void ThrowSword()
    {
        SkillManager.Instance.Sword.CreateSword();
    }
}
