using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningController : MonoBehaviour
{
    [SerializeField] 
    private EntityStats entityStats;
    [SerializeField] 
    private float lightningSpeed;

    private Animator anim;
    private bool triggered;
    private int damage;
    
    private static readonly int Hit = Animator.StringToHash("hit");
    private static readonly int MoveX = Animator.StringToHash("moveX");
    private static readonly int MoveY = Animator.StringToHash("moveY");

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        LightningAnimation();
    }

    private void Update()
    {
        if (entityStats == null)
        {
            return;
        }
        
        if (triggered)
        {
            return;
        }
        
        LightningAnimation();
        
        transform.position = Vector2.MoveTowards(transform.position, entityStats.transform.position,
            lightningSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, entityStats.transform.position) < 0.1f)
        {
            Invoke(nameof(DamageAndDestroy), .2f);
            
            triggered = true;
            anim.SetTrigger(Hit);
        }
    }

    public void SetupLightning(int damage, EntityStats entityStats)
    {
        this.damage = damage;
        this.entityStats = entityStats;
    }
    
    private void LightningAnimation()
    {
        anim.SetFloat(MoveX, entityStats.transform.position.x - transform.position.x);
        anim.SetFloat(MoveY, entityStats.transform.position.y - transform.position.y);
    }

    private void DamageAndDestroy()
    {
        entityStats.TakeDamage(damage);
        Destroy(gameObject, 1f);
    }
}
