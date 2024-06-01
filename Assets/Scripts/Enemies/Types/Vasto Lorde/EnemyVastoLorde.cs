using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyVastoLorde : Enemy
{
    [Header("Cero Details")] 
    [SerializeField]
    private GameObject ceroPrefab;
    public float ceroCooldown;

    private float lastTimeCast;
    
    [SerializeField] 
    private RoomCenter roomEnd;
    [SerializeField] 
    private BoxCollider2D endRoomCollider;
    public GameObject portal;

    public float chanceToTeleport;
    public float defaultChanceToTeleport = 25;
    
    #region States
    public VastoLordeIdleState OnIdleState { get; private set; }
    public VastoLordeMoveState OnMoveState { get; private set; }
    public VastoLordeBattleState OnBattleState { get; private set; }
    public VastoLordeAttackState OnAttackState { get; private set; }
    public VastoLordeDeathState OnDeathState { get; private set; }
    public VastoLordeTeleportState OnTeleportState { get; private set; }
    public VastoLordeSpellCastState OnCastState { get; private set; }
    #endregion
    
    protected override void Awake()
    {
        base.Awake();
        
        OnIdleState = new VastoLordeIdleState(this, OnStateMachine, "idle", this);
        OnMoveState = new VastoLordeMoveState(this, OnStateMachine, "walk", this);
        OnBattleState = new VastoLordeBattleState(this, OnStateMachine, "walk", this);
        OnAttackState = new VastoLordeAttackState(this, OnStateMachine, "attack", this);
        OnDeathState = new VastoLordeDeathState(this, OnStateMachine, "death", this);
        OnTeleportState = new VastoLordeTeleportState(this, OnStateMachine, "teleport", this);
        OnCastState = new VastoLordeSpellCastState(this, OnStateMachine, "cero", this);
    }

    protected override void Start()
    {
        base.Start();
        
        endRoomCollider = roomEnd.TheRoom.GetComponentInChildren<BoxCollider2D>();
        OnStateMachine.Initialize(OnIdleState);
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void EntityDeath()
    {
        base.EntityDeath();
        OnStateMachine.ChangeState(OnDeathState);
    }

    public override void AttackDirection(Vector2 movementInput)
    {
        if (Mathf.Abs(movementInput.x) > Mathf.Abs(movementInput.y))
        {
            if (movementInput.x > 0)
            {
                OnAttackDirection = new Vector2(3, 0f);
            }
            if (movementInput.x < 0)
            {
                OnAttackDirection = new Vector2(-1f, 0f);
            }
        }
        if (Mathf.Abs(movementInput.x) < Mathf.Abs(movementInput.y))
        {
            if (movementInput.y > 0)
            {
                OnAttackDirection = new Vector2(1f, 1f);
            }
            if (movementInput.y < 0)
            {
                OnAttackDirection = new Vector2(1f, -0.5f);
            }
        }
    }

    public void FindPosition()
    {
        if (Time.time >= lastTimeCast + ceroCooldown)
        {
            transform.position = PlayerManager.Instance.player.transform.position;
        }
        else
        {
            float xPos = Random.Range(endRoomCollider.bounds.min.x + 3, endRoomCollider.bounds.max.x - 3);
            float yPos = Random.Range(endRoomCollider.bounds.min.y + 3, endRoomCollider.bounds.max.y - 3);

            transform.position = new Vector3(xPos, yPos);
        }
    }

    public bool CanTeleport()
    {
        if (Random.Range(0, 100) <= chanceToTeleport)
        {
            chanceToTeleport = defaultChanceToTeleport;
            return true;
        }

        return false;
    }

    public bool CanDoSpell()
    {
        if (Time.time >= lastTimeCast + ceroCooldown)
        {
            lastTimeCast = Time.time;
            return true;
        }
        
        return false;
    }

    public void CastCero()
    {
        GameObject newCero = Instantiate(ceroPrefab, new Vector3(transform.position.x, transform.position.y + 1), quaternion.identity);
        newCero.GetComponent<LaserController>().SetupLaser(animatorDirection.x, animatorDirection.y, OnEntityStats, gameObject);
    }

    public override void DamageEffect(GameObject sender)
    {
        OnEntityFx.StartCoroutine("FlashEffects");
    }
}
