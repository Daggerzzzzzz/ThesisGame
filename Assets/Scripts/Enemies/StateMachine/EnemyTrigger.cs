using System;
using UnityEngine;

public class EnemyTrigger : MonoBehaviour
{
    [SerializeField] 
    private GameObject darkPillar;
    
    private Enemy OnEnemy => GetComponentInParent<Enemy>();
    private bool attackOnce;

    [SerializeField] 
    private string enemyName;

    private void Start()
    {
        enemyName = OnEnemy.gameObject.name;
    }

    private void EnemyAnimation()
    {
        OnEnemy.AnimationTriggerForEnemy();
        ResetAttack();
    }
    
    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(OnEnemy.attackCheck.position, OnEnemy.attackCheckRadius);
        
        foreach (var hit in colliders)
        {
            if (hit.CompareTag("Player Trigger Collider") && !attackOnce)
            {
                if (enemyName == "slimes")
                {
                    SoundManager.Instance.PlaySoundEffects(16, null, false);
                    
                }
                else if(enemyName == "zombies")
                {
                    SoundManager.Instance.PlaySoundEffects(15, null, false);
                }
                
                PlayerStats target = hit.GetComponentInParent<PlayerStats>();
                OnEnemy.OnEntityStats.DoDamage(target, gameObject);
                attackOnce = true;
            }
        }
    }

    private void MagicTrigger()
    {
        GameObject newDarkPillar = Instantiate(darkPillar, PlayerManager.Instance.player.transform.position, Quaternion.identity);
        newDarkPillar.GetComponent<DarkPillarController>().SetUpDarkPillar(OnEnemy.OnEntityStats, gameObject);
        SoundManager.Instance.PlaySoundEffects(34, null, false);
    }

    private void ResetAttack()
    {
        attackOnce = false;
    }
}
