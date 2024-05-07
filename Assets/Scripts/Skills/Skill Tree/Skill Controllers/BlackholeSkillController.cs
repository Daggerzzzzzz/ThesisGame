using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class BlackholeSkillController : MonoBehaviour
{
    [SerializeField]
    private GameObject swordPrefab;
    [SerializeField]
    private List<GameObject> slashesList;

    public bool PlayerCanExitUltimate { get; private set; }
    
    private float maximumSize;
    private float speedOfGrowth;
    private float swordAttackCooldown;
    private float swordAttackTimer;
    private float summonTimer; 
    private float angleIncrement;
    private float circleRadius;
    private float angleIncrementRad;
    private float currentAngleRad;
    
    private bool canGrow = true;
    private bool canShrink;
    private bool canAttack;
    private bool allSwordsSummoned;
    private bool alreadyScattered = false;
    
    private int amountOfSwords;
    private int amountOfAttacks;
    private int swordsSummoned; 
    
    private readonly List<Transform> targets = new();
    [SerializeField]
    private List<GameObject> swords = new();
    private GameObject newSlash;
    private CircleCollider2D circleCollider2D;
    private Player player;
    
    private static readonly int Scatter = Animator.StringToHash("scatter");

    private void Awake()
    {
        circleCollider2D = GetComponent<CircleCollider2D>();
    }

    private void Start()
    {
        circleRadius = circleCollider2D.radius * maximumSize;
        currentAngleRad = 0f;
        angleIncrementRad = angleIncrement * Mathf.Deg2Rad;
        amountOfAttacks = amountOfSwords / 2;
        allSwordsSummoned = false;
    }

    private void Update()
    {
        swordAttackTimer -= Time.deltaTime;

        if (allSwordsSummoned)
        {
            canAttack = true;
            if (!alreadyScattered)
            {
                foreach (var sword in swords)
                {
                    sword.GetComponent<Animator>().SetTrigger(Scatter);
                }

                alreadyScattered = true;
            }
        }

        if (swords.Count != 0)
        {
            for (int i = 0; i < swords.Count; i++)
            {
                if (swords[i] == null)
                {
                    swords.Remove(swords[i]);
                }
            }
        }
        
        if (swordAttackTimer < 0 && canAttack)
        {
            Debug.Log("Pumasok Dito");
            swordAttackTimer = swordAttackCooldown;
            
            if (targets.Count == 0)
            {
                PlayerCanExitUltimate = true;
                canAttack = false;
                canShrink = true;
                return;
            }

            if (newSlash == null)
            {
                SlashAttack();
            }
        }
        
        if (canGrow && !canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maximumSize, maximumSize), speedOfGrowth * Time.deltaTime);
            StartCoroutine(SlashAnimationDelay());
            BulkSwordSummon();
        }
        
        if (swords.Count == 0)
        {
            PlayerCanExitUltimate = true;
            canAttack = false;
            canShrink = true;
        }
        
        DestroyBlackhole();
    }

    private void SlashAttack()
    {
        if (amountOfAttacks > 0)
        {
            int randomSlash = UnityEngine.Random.Range(0, slashesList.Count);
            int randomTarget = UnityEngine.Random.Range(0, targets.Count);
            newSlash = Instantiate(slashesList[randomSlash], targets[randomTarget].transform.position, Quaternion.identity);
            newSlash.GetComponent<SlashController>().SetupSwordSlash(player);
            amountOfAttacks--;
        }
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

    private void SummonSword()
    {
        float x = transform.position.x + circleRadius * Mathf.Cos(currentAngleRad);
        float y = transform.position.y + circleRadius * Mathf.Sin(currentAngleRad);
        Vector3 spawnPosition = new Vector3(x, y, 0f);

        GameObject newSword = Instantiate(swordPrefab, spawnPosition, Quaternion.identity);
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
                SummonSword();
                summonTimer = swordAttackCooldown;
            }
        }
    }

    private void DestroyBlackhole()
    {
        if (canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), speedOfGrowth * Time.deltaTime);

            if (transform.localScale.x <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private IEnumerator SlashAnimationDelay()
    {
        yield return new WaitForSeconds(swordAttackCooldown * amountOfSwords);
        allSwordsSummoned = true;
    }

    public void SetupBlackhole(float maximumSize, float speedOfGrowth, int amountOfSwords, float swordAttackCooldown, float angleIncrement, Player player)
    {
        this.maximumSize = maximumSize;
        this.speedOfGrowth = speedOfGrowth;
        this.amountOfSwords = amountOfSwords;
        this.swordAttackCooldown = swordAttackCooldown;
        this.angleIncrement = angleIncrement;
        this.player = player;
    }
}
