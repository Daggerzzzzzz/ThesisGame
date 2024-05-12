using System.Collections.Generic;
using UnityEngine;

public class SwordSkillController : MonoBehaviour
{
    [Header("Bounce Info")]
    private float bounceSpeed;

    [Header("Spin Info")] 
    private float maxDistance;
    private float spinDuration;
    private float spinTimer;
    private float onHitTimer;
    private float onHitCooldown;
    private bool wasStopped;
    private bool isSpinning;

    [Header("Freeze Time Info")] 
    private float freezeTimeDuration;
    
    private Animator anim;
    private CircleCollider2D circleCollider;
    private Rigidbody2D rb;
    private Player player;

    private bool canRotate;
    private bool isReturning;
    private bool canChangeSwordPosition;
    private bool isBouncing;
    
    private Vector2 playerFacingDirection;
    private Vector3 targetPosition;
    
    private float throwSword;
    private float swordSpeed;
    
    private int targetIndex;
    private int amountOfBounce;
    
    private List<Transform> enemyTargets;
    
    private static readonly int Rotation = Animator.StringToHash("rotation");
    private static readonly int MoveX = Animator.StringToHash("moveX");
    private static readonly int MoveY = Animator.StringToHash("moveY");
    private static readonly int Stuck = Animator.StringToHash("stuck");

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        circleCollider = GetComponentInChildren<CircleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        canRotate = true;
    }

    private void Start()
    {
        canChangeSwordPosition = true;
    }

    private void Update()
    {
        if (canRotate)
        {
            rb.velocity = playerFacingDirection.normalized * throwSword;
        }

        if (canChangeSwordPosition)
        {
            if (player.animatorDirection != Vector2.zero)
            {
                anim.SetFloat(MoveX, player.animatorDirection.x);
                anim.SetFloat(MoveY, player.animatorDirection.y);
                canChangeSwordPosition = false;
            }
        }
        
        if (isReturning)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, swordSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, player.transform.position) < 1)
            {
                player.ClearNewSword();
            }
        }

        BounceSkill();
        SpinSkill();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isReturning)
        {
            return;
        }

        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            SwordSkillDamage(enemy);
        }
        
        EnemyTargetSelection(other);
        SwordStuck(other);
    }
    
    public void InitializeSword(Vector2 movementDirection, float throwForce, float returnSpeed, Player player, float freezeTimeDuration)
    {
        this.player = player;
        this.freezeTimeDuration = freezeTimeDuration;
        throwSword = throwForce;
        swordSpeed = returnSpeed;
        playerFacingDirection = movementDirection;
        rb.velocity = movementDirection;
        anim.SetBool(Rotation, true);
        targetPosition = playerFacingDirection * 20f;
        Invoke(nameof(DestroySword), 10f);
    }

    public void InitializeSpin(bool isSpinning, float maxDistance, float spinDuration, float onHitCooldown)
    {
        this.isSpinning = isSpinning;
        this.maxDistance = maxDistance;
        this.spinDuration = spinDuration;
        this.onHitCooldown = onHitCooldown;
    }
    
    public void InitializeBounce(bool isBouncing, int amountOfBounce, float bounceSpeed)
    {
        this.isBouncing = isBouncing;
        this.amountOfBounce = amountOfBounce;
        this.bounceSpeed = bounceSpeed;
        enemyTargets = new List<Transform>();
    }

    public void ReturnSword()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.parent = null;
        isReturning = true;
        anim.SetBool(Stuck, false);
        
        //player.OnSword.
        ResetAnimator();
    }
    
    private void ResetAnimator()
    {
        canChangeSwordPosition = true;
    }

    private void SwordStuck(Collider2D other)
    {
        canRotate = false;
        circleCollider.enabled = false;
        
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        if (isBouncing && enemyTargets.Count > 0)
        {
            return;
        }

        if (isSpinning)
        {
            StopWhenEnemiesEncountered();
            return;
        }
        
        transform.parent = other.transform;
        anim.SetBool(Rotation, false);
        anim.SetBool(Stuck, true);
    }

    private void StopWhenEnemiesEncountered()
    {
        wasStopped = true;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        spinTimer = spinDuration;
    }

    private void BounceSkill()
    {
        if (isBouncing && enemyTargets.Count > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, enemyTargets[targetIndex].position, bounceSpeed * Time.deltaTime);
            
            if (Vector2.Distance(transform.position, enemyTargets[targetIndex].position) < .1f)
            {
                SwordSkillDamage(enemyTargets[targetIndex].GetComponent<Enemy>());
                targetIndex++;
                amountOfBounce--;

                if (amountOfBounce <= 0)
                {
                    isBouncing = false;
                    isReturning = true;
                }

                if (targetIndex >= enemyTargets.Count)
                {
                    targetIndex = 0;
                }
            }
        }
    }

    private void SpinSkill()
    {
        if (isSpinning)
        {
            if (Vector2.Distance(player.transform.position, transform.position) > maxDistance && !wasStopped)
            {
                StopWhenEnemiesEncountered();
            }
            
            if (wasStopped)
            {
                spinTimer -= Time.deltaTime;
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + targetPosition.x, transform.position.y + targetPosition.y), 1.5f * Time.deltaTime);
                
                if (spinTimer < 0)
                {
                    isSpinning = false;
                    isReturning = true;
                }

                onHitTimer -= Time.deltaTime;
                
                if (onHitTimer < 0)
                {
                    onHitTimer = onHitCooldown;
                    
                    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.75f);
                    foreach (var hit in colliders)
                    {
                        if (hit.CompareTag("Enemy"))
                        {
                            SwordSkillDamage(hit.GetComponent<Enemy>());
                        }
                    }
                }
            }
        }
    }

    private void EnemyTargetSelection(Collider2D other)
    {
        if (other.GetComponent<Enemy>() != null)
        {
            if (isBouncing && enemyTargets.Count <= 0)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10);
                foreach (var hit in colliders)
                {
                    if (hit.CompareTag("Enemy"))
                    {
                        if (!enemyTargets.Contains(hit.transform))
                        {
                            enemyTargets.Add(hit.transform);
                        }
                    }
                }
            }
        }
    }

    private void SwordSkillDamage(Enemy enemy)
    {
        EnemyStats enemyStats = enemy.GetComponent<EnemyStats>();
        
        player.OnEntityStats.DoDamage(enemyStats);

        if (player.OnSkill.Sword.timeStopUnlocked)
        { 
            enemy.StartCoroutine("FreezeTimeFor", freezeTimeDuration);
        }
        
        if (player.OnSkill.Sword.vulnerableUnlocked)
        { 
            enemyStats.MakeVulnerable(freezeTimeDuration);
        }
    }

    private void DestroySword()
    {
        Destroy(gameObject);
    }
}
