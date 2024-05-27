using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.LowLevel;

public class PlayerTrigger : MonoBehaviour
{
    [SerializeField] 
    private GameObject lightningPrefab;
    
    private Player OnPlayer => GetComponentInParent<Player>();
    
    private bool attackOnce;
    
    private static readonly int Resurrection = Animator.StringToHash("resurrection");

    private void PlayerAnimation()
    {
        OnPlayer.AnimationTriggerForPlayer();
        ResetAttack();
    }

    private void AttackTrigger()
    {
        SoundManager.Instance.PlaySoundEffects(7, null);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(OnPlayer.attackCheck.position, OnPlayer.attackCheckRadius);
        EquipmentDataSO currentWeapon;
        EquipmentDataSO currentArmor;
        foreach (var hit in colliders)
        {
            if (hit.CompareTag("Enemy") && !attackOnce)
            {
                SoundManager.Instance.PlaySoundEffects(8, null);
                EnemyStats target = hit.GetComponent<EnemyStats>();
                
                OnPlayer.OnEntityStats.DoDamage(target,gameObject);
                
                currentWeapon = Inventory.Instance.GetEquipment(EquipmentType.WEAPON);
                if (currentWeapon.itemName == "Gyorinmaru")
                {
                    Inventory.Instance.GetEquipment(EquipmentType.WEAPON).UseEffect(target.transform.position, OnPlayer.GetComponent<PlayerStats>());
                }
                
                currentArmor = Inventory.Instance.GetEquipment(EquipmentType.ARMOR);
                if (currentArmor.itemName == "Warmogs")
                {
                    Inventory.Instance.GetEquipment(EquipmentType.ARMOR).UseEffect(target.transform.position, OnPlayer.GetComponent<PlayerStats>());
                }
                attackOnce = true;
            }
        }
    }

    private void DisableResurrectionDelay()
    {
        OnPlayer.OnCapsuleCollider2D.enabled = true;
        OnPlayer.OnBoxCollider2D.enabled = true;
        OnPlayer.OnAnim.SetBool(Resurrection, false);
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
