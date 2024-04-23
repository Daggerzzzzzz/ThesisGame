using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BlackholeSkillController : MonoBehaviour
{
    [SerializeField]
    private GameObject swordPrefab;
    [SerializeField]
    private List<GameObject> slashesList;
    
    private float maximumSize;
    private float speedOfGrowth;
    
    private bool canGrow = true;
    private bool canShrink;
    
    private int amountOfSwords;
    private float swordAttackCooldown;
    private float amountOfAttacks;
    private float swordAttackTimer;
    private bool canAttack;
    private int swordsSummoned; 
    private float summonTimer; 
    
    private float angleIncrement;
    private float circleRadius;
    private float angleIncrementRad;
    private float currentAngleRad;
    
    private readonly List<Transform> targets = new();
    private readonly List<GameObject> slashes = new();
    private readonly List<GameObject> swords = new();
    private CircleCollider2D collider;
    private Animator anim;

    private void Awake()
    {
        collider = GetComponent<CircleCollider2D>();
    }

    private void Start()
    {
        circleRadius = collider.radius * maximumSize;
        currentAngleRad = 0f;
        angleIncrementRad = angleIncrement * Mathf.Deg2Rad;
        amountOfAttacks = amountOfSwords / 2;
    }

    private void Update()
    {
        swordAttackTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.R))
        {
            canAttack = true;
            
        }
        
        if (swordAttackTimer < 0 && canAttack)
        {
            if (targets.Count == 0)
            {
                canAttack = false;
                canShrink = true;
                PlayerManager.Instance.player.ExitBlackhole();
            }
            
            swordAttackTimer = swordAttackCooldown;
            
            int randomSlash = UnityEngine.Random.Range(0, slashesList.Count);
            int randomTarget = UnityEngine.Random.Range(0, targets.Count);
            GameObject newSlash = Instantiate(slashesList[randomSlash], targets[randomTarget].transform.position, Quaternion.identity);
            slashes.Add(newSlash);
            
            amountOfAttacks--;

            if (amountOfAttacks <= 0)
            {
                canAttack = false;
                canShrink = true;
                PlayerManager.Instance.player.ExitBlackhole();
            }
        }
        
        if (canGrow && !canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maximumSize, maximumSize), speedOfGrowth * Time.deltaTime);
            BulkSwordSummon();
        }
        
        DestroyBlackhole();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (!targets.Contains(other.transform))
            {
                targets.Add(other.transform);
            }
        }

        foreach (Transform target in targets)
        {
            target.GetComponent<Enemy>().TimeFreeze(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        foreach (Transform target in targets)
        {
            target.GetComponent<Enemy>().TimeFreeze(false);
        }
    }

    private void SummonSwords()
    {
        float x = transform.position.x + circleRadius * Mathf.Cos(currentAngleRad);
        float y = transform.position.y + circleRadius * Mathf.Sin(currentAngleRad);
        Vector3 spawnPosition = new Vector3(x, y, 0f);

        GameObject newSword = Instantiate(swordPrefab, spawnPosition, Quaternion.identity);
        anim = newSword.GetComponent<Animator>();
        swords.Add(newSword);
            
        currentAngleRad += angleIncrementRad;
        swordsSummoned++;
    }

    private void BulkSwordSummon()
    {
        if (swordsSummoned < amountOfSwords)
        {
            summonTimer -= Time.deltaTime;
                
            if (summonTimer <= 0)
            {
                SummonSwords();
                summonTimer = swordAttackCooldown;
            }
        }
    }
    
    private void DestroySword()
    {
        if (swords.Count <= 0)
        {
            return;
        }

        foreach (var sword in swords)
        {
            Destroy(sword);
        }
    }

    private void DestroyBlackhole()
    {
        if (canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), speedOfGrowth * Time.deltaTime);

            if (transform.localScale.x < 0)
            {
                DestroySword();
                Destroy(gameObject);
            }
        }
    }

    public void SetupBlackhole(float maximumSize, float speedOfGrowth, int amountOfSwords, float swordAttackCooldown, float angleIncrement)
    {
        this.maximumSize = maximumSize;
        this.speedOfGrowth = speedOfGrowth;
        this.amountOfSwords = amountOfSwords;
        this.swordAttackCooldown = swordAttackCooldown;
        this.angleIncrement = angleIncrement;
    }
}
