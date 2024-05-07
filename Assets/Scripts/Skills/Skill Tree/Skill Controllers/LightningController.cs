using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningController : MonoBehaviour
{
    [SerializeField] 
    private EntityStats entityStats;
    [SerializeField] 
    private LayerMask enemyLayer;
    [SerializeField] 
    private GameObject lightningController;
    [SerializeField] 
    private GameObject beenStruck;
    [SerializeField] 
    private GameObject endObject;
    [SerializeField] 
    private int amountToChain;

    private Animator anim;
    private CircleCollider2D circleCollider2D;
    [SerializeField] 
    private GameObject startObject;
    private ParticleSystem part;
    
    private int damage;
    private int singleSpawns;
    
    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        part = GetComponent<ParticleSystem>();
        circleCollider2D = GetComponent<CircleCollider2D>();
        startObject = gameObject;
    }

    private void Start()
    {
        if (amountToChain == 0)
        {
            Destroy(gameObject);
        }

        singleSpawns = 1;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("damage" + damage);
        if (enemyLayer == (enemyLayer | (1 << other.gameObject.layer)) && !other.GetComponentInChildren<EnemyStruck>())
        { 
            if (singleSpawns != 0)
            {
                endObject = other.gameObject;
                amountToChain -= 1;
            
                GameObject newLightning = Instantiate(lightningController, other.gameObject.transform.position, Quaternion.identity);
                newLightning.GetComponent<LightningController>().SetupLightning(damage);
                Instantiate(beenStruck, other.gameObject.transform);
                entityStats = other.GetComponent<EntityStats>();
            
                anim.StopPlayback();
                DamageAndDestroy();
                ParticleAnimation();
                singleSpawns--;
            }
        }
    }

    public void SetupLightning(int damage)
    {
        this.damage = damage;
    }
    
    private void DamageAndDestroy()
    {
        circleCollider2D.enabled = false;
        entityStats.TakeDamage(damage);
        Destroy(gameObject, 1f);
    }

    private void ParticleAnimation()
    {
        part.Play();
        var emitParams = new ParticleSystem.EmitParams();
        
        emitParams.position = startObject.transform.position;
        part.Emit(emitParams, 1);
        
        emitParams.position = endObject.transform.position;
        part.Emit(emitParams, 1);

        emitParams.position = (startObject.transform.position + endObject.transform.position) / 2;
        part.Emit(emitParams, 1);
    }
}
