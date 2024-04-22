using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerTrigger : MonoBehaviour
{
    private Player OnPlayer => GetComponentInParent<Player>();
    
    private readonly HashSet<Enemy> attackedEnemies = new();
    
    private void PlayerAnimation()
    {
        OnPlayer.AnimationTriggerForPlayer();
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(OnPlayer.attackCheck.position, OnPlayer.attackCheckRadius);
        foreach (var hit in colliders)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy != null)
            {
                if (!attackedEnemies.Contains(enemy))
                {
                    enemy.Damage();
                    attackedEnemies.Add(enemy);
                }
            }
        }
    }

    private void ResetColliders()
    {
        attackedEnemies.Clear();
    }

    private void ThrowSword()
    {
        SkillManager.Instance.sword.CreateSword();
    }
}
