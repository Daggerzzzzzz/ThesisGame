using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieTrigger : MonoBehaviour
{
    private EnemyZombie OnEnemyZombie => GetComponentInParent<EnemyZombie>();
    private HashSet<Player> playerColliders = new();
    private bool alreadyDamaged = false;
    
    private void EnemyAnimation()
    {
        OnEnemyZombie.AnimationTriggerForEnemy();
        playerColliders.Clear();
    }
    
    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(OnEnemyZombie.attackCheck.position, OnEnemyZombie.attackCheckRadius);
        foreach (var hit in colliders)
        {
            Player player = hit.GetComponent<Player>();
            if (player != null)
            {
                if (!playerColliders.Contains(player))
                {
                    PlayerStats target = hit.GetComponent<PlayerStats>();
                    OnEnemyZombie.OnEntityStats.DoDamage(target);
                    playerColliders.Add(player);
                }
            }
        }
    }
    
    private void ResetColliders()
    {
        playerColliders.Clear();
    }
}
