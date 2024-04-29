using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class KunaiSkillController : MonoBehaviour
{
    private Animator anim;
    private CircleCollider2D collider;

    private bool canExplode;
    private bool canGrow;
    private bool canTeleport;
    private bool canMultiStack;
    
    private float speedOfGrowth = 3;

    public bool pressedTwice { get; set; }

    private static readonly int CanExplode = Animator.StringToHash("canExplode");
    
    private void Awake()
    {
        anim = GetComponent<Animator>();
        collider = GetComponent<CircleCollider2D>();
    }

    private void Start()
    {
        canTeleport = true;
    }

    public void SetupCrystal(bool canExplode, bool canTeleport,bool pressedTwice, bool canMultiStack)
    {
        this.canExplode = canExplode;
        this.canTeleport = canTeleport;
        this.pressedTwice = pressedTwice;
        this.canMultiStack = canMultiStack;
    }

    private void Update()
    {
        if (canGrow)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(3, 3), speedOfGrowth * Time.deltaTime);
        }

        if (canTeleport)
        {
            if (!pressedTwice)
            {
                return;
            }
            
            KunaiExplosion();
        }

        if (canExplode)
        {
            if (!pressedTwice)
            {
                return;
            }
            
            KunaiExplosion();
        }

        if (canMultiStack)
        {
            if (!pressedTwice)
            {
                return;
            }
            
            KunaiExplosion();
        }
    }

    public void KunaiExplosion()
    {
        if (canExplode || canMultiStack)
        {
            canGrow = true;
            anim.SetBool(CanExplode, true);
        }
        else
        {
            KunaiDestroy();
        }
    }
    
    public void KunaiDestroy()
    {
        Destroy(gameObject);
    }

    public void ExplodeEvent()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, collider.radius);

        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                collider.GetComponent<Enemy>().DamageEffect();
            }
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 1);
    }
}
