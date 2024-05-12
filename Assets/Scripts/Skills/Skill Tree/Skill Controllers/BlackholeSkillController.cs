using System.Collections.Generic;
using UnityEngine;

public class BlackholeSkillController : MonoBehaviour
{
    [SerializeField]
    private GameObject swordPrefab;
    [SerializeField]
    private List<GameObject> slashesList;
    [SerializeField]
    private List<GameObject> swords = new();

    public bool PlayerCanExitUltimate { get; private set; }
    
    [Header("Sword Info")]
    private float swordAttackCooldown;
    private float swordAttackTimer;
    private int amountOfSwords;
    private int swordsSummoned; 
    private bool allSwordsSummoned;
    private bool alreadyScattered;
    private bool alreadyDisappear;
    private bool alreadyDestroyed;
    
    [Header("Slash Info")]
    private float canSlashAttackTimer;
    private float slashAttackTimer;
    private float slashAttackCooldown;
    private int amountOfSlashAttacks;
    private int numberOfSlashAttacksExecuted;
    
    [Header("Blackhole Info")]
    private float maximumSize;
    private float speedOfGrowth;
    private float angleIncrement;
    private float circleRadius;
    private float angleIncrementRad;
    private float currentAngleRad;
    private bool canGrow;
    private bool canShrink;
    private bool canAttack;
    
    
    private List<GameObject> targets = new();
    private CircleCollider2D circleCollider2D;
    private Player player;
    
    private static readonly int Scatter = Animator.StringToHash("scatter");
    private static readonly int NoEnemies = Animator.StringToHash("noEnemies");
    private static readonly int CanDestroy = Animator.StringToHash("canDestroy");

    private void Awake()
    {
        circleCollider2D = GetComponent<CircleCollider2D>();
    }

    private void Start()
    {
        circleRadius = circleCollider2D.radius * maximumSize;
        angleIncrementRad = angleIncrement * Mathf.Deg2Rad;
        canSlashAttackTimer = swordAttackCooldown * amountOfSwords + 0.25f;
        allSwordsSummoned = false;
        alreadyScattered = false;
        alreadyDisappear = false;
        canGrow = true;
        currentAngleRad = 0f;
        slashAttackCooldown = 0.5f;
        amountOfSlashAttacks = amountOfSwords / 2;
    }

    private void Update()
    {
        canSlashAttackTimer -= Time.deltaTime;

        CheckForMissingObjects(swords);
        
        CheckForMissingObjects(targets);
        
        CheckToSeeIfTheSwordWillAttackOrDisappear();
        
        if (canAttack)
        {
            BulkSlashSummon();
        }
        
        if (canGrow && !canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maximumSize, maximumSize), speedOfGrowth * Time.deltaTime);
            
            if (canSlashAttackTimer <= 0)
            {
                allSwordsSummoned = true;
            }
            
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (!targets.Contains(other.transform.gameObject))
            {
                targets.Add(other.transform.gameObject);
            }
        }

        foreach (GameObject target in targets)
        {
            target.GetComponent<Enemy>().TimeFreeze(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        foreach (GameObject target in targets)
        {
            if (target == null)
            {
                return;
            }
            target.GetComponent<Enemy>().TimeFreeze(false);
        }
    }

    private void BulkSwordSummon()
    {
        if (swordsSummoned < amountOfSwords)
        {
            swordAttackTimer -= Time.deltaTime;
                
            if (swordAttackTimer <= 0)
            {
                SummonSword();
                swordAttackTimer = swordAttackCooldown;
            }
        }
    }
    
    private void BulkSlashSummon()
    {
        if (targets.Count == 0)
        {
            numberOfSlashAttacksExecuted = amountOfSlashAttacks;
        }
        
        if (numberOfSlashAttacksExecuted <= amountOfSlashAttacks)
        {
            slashAttackTimer -= Time.deltaTime;

            if (slashAttackTimer <= 0)
            {
                slashAttackTimer = slashAttackCooldown;
                SlashAttack(); 
            }
        }
        else
        {
            if (!alreadyDestroyed)
            {
                foreach (var sword in swords)
                {
                    sword.GetComponent<Animator>().SetTrigger(CanDestroy);
                }

                alreadyDestroyed = true;
            }
        }
    }
    
    private void CheckToSeeIfTheSwordWillAttackOrDisappear()
    {
        if (allSwordsSummoned)
        {
            if (targets.Count == 0)
            {
                if (!alreadyDisappear)
                {
                    foreach (var sword in swords)
                    {
                        sword.GetComponent<Animator>().SetTrigger(NoEnemies);
                    }   
                }
                
                canAttack = false;
                alreadyDisappear = true;
            }
            else if (targets.Count != 0)
            {
                if (!alreadyScattered)
                {
                    foreach (var sword in swords)
                    {
                        sword.GetComponent<Animator>().SetTrigger(Scatter);
                    }
                
                    canAttack = true;
                    alreadyScattered = true;
                }
            }
        }
    }
    
    private void SlashAttack()
    {
        Debug.Log(numberOfSlashAttacksExecuted);
        int randomSlash = UnityEngine.Random.Range(0, slashesList.Count);
        int randomTarget = UnityEngine.Random.Range(0, targets.Count);
        
        if (targets[randomTarget] != null)
        {
            if (targets[randomTarget].GetComponent<EntityStats>().currentHealth <= 0)
            {
                numberOfSlashAttacksExecuted++;
                slashAttackTimer = 0;
                return;
            }
            numberOfSlashAttacksExecuted++;
            GameObject newSlash = Instantiate(slashesList[randomSlash], targets[randomTarget].transform.position, Quaternion.identity);
            newSlash.GetComponent<SlashController>().SetupSwordSlash(player);
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
    
    private void CheckForMissingObjects(List<GameObject> gameObjects)
    {
        for (int i = 0; i < gameObjects.Count; i++)
        {
            if (gameObjects[i] == null)
            {
                gameObjects.Remove(gameObjects[i]);
            }
        }
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
