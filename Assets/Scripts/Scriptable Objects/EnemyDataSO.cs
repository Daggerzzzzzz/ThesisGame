using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyData")]
public class EnemyDataSO : ScriptableObject
{
    [field: SerializeField]
    public int MaxHealth { get; set; } = 3;
    [field: SerializeField]
    public int Damage { get; set; } = 1;
    
    [SerializeField]
    public float enemyMaterializeTime;
    [SerializeField]
    public Material enemyStandardMaterial;
}
