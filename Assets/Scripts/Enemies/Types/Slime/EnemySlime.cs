using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public enum SlimeType
{
    BIG,
    MEDIUM,
    SMALL
}

public class EnemySlime : Enemy
{
    [Header("Slime Specific")] 
    [SerializeField]
    private SlimeType slimeType;
    [SerializeField]
    private GameObject slimePrefab;
    [SerializeField]
    private int amountOfSlimes;
    [SerializeField]
    private Vector2 minCreationVelocity;
    [SerializeField]
    private Vector2 maxCreationVelocity;
    [SerializeField]
    private RoomCenter roomCenter;
    
    #region States
    public SlimeIdleState OnIdleState { get; private set; }
    public SlimeMoveState OnMoveState { get; private set; }
    public SlimeBattleState OnBattleState { get; private set; }
    public SlimeAttackState OnAttackState { get; private set; }
    public SlimeDeathState OnDeathState { get; private set; }
    #endregion
    
    protected override void Awake()
    {
        base.Awake();
        OnIdleState = new SlimeIdleState(this, OnStateMachine, "idle", this);
        OnMoveState = new SlimeMoveState(this, OnStateMachine, "walk", this);
        OnBattleState = new SlimeBattleState(this, OnStateMachine, "walk", this);
        OnAttackState = new SlimeAttackState(this, OnStateMachine, "attack", this);
        OnDeathState = new SlimeDeathState(this, OnStateMachine, "death", this);
    }

    protected override void Start()
    {
        base.Start();
        roomCenter = GetComponentInParent<RoomCenter>();
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
        
        if (slimeType == SlimeType.SMALL)
        {
            return;
        }
        
        CreateSlimes(amountOfSlimes, slimePrefab);
    }

    private void CreateSlimes(int amountOfSlimes, GameObject slimePrefab)
    {
        for (int i = 0; i < amountOfSlimes; i++)
        {
            float xVelocity = Random.Range(minCreationVelocity.x, maxCreationVelocity.x);
            float yVelocity = Random.Range(minCreationVelocity.y, maxCreationVelocity.y);

            Vector3 randomPos = new Vector3(xVelocity, yVelocity);
            Debug.Log(randomPos);
            
            GameObject newSlime = Instantiate(slimePrefab, transform.position + randomPos, quaternion.identity);
            newSlime.transform.parent = roomCenter.transform;
            roomCenter.enemies.Add(newSlime);
        }
    }
}
