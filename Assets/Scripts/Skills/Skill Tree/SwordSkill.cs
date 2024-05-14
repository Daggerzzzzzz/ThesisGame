using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public enum SwordType
{
    REGULAR,
    BOUNCE,
    SPIN
}

public class SwordSkill : Skill
{
    [Header("Sword Type")] 
    public SwordType swordType = SwordType.REGULAR;
    
    [Header("Flying Sword Skill Info")]
    [SerializeField]
    private SkillTreeSlot swordFlyingUnlockButton;
    public bool swordFlyingUnlocked { get; private set; }

    [SerializeField]
    private GameObject swordPrefab;
    [SerializeField] 
    private float throwForce = 15;
    [SerializeField] 
    private float returnSpeed = 15;

    [Header("Bounce Sword Skill Info")] 
    [SerializeField]
    private SkillTreeSlot swordBounceUnlockButton;
    [SerializeField]
    private int amountOfBounce;
    [SerializeField]
    private int bounceSpeed;
    
    [Header("Spin Sword Skill Info")] 
    [SerializeField]
    private SkillTreeSlot swordSpinUnlockButton;
    [SerializeField]
    private float maxDistance = 7;
    [SerializeField] 
    private float spinDuration = 2;
    [SerializeField] 
    private float onHitCooldown = 0.25f;
    
    [Header("Passive Skills")] 
    [SerializeField]
    private SkillTreeSlot timeStopUnlockedButton; 
    public bool TimeStopUnlocked { get; private set; }
    [SerializeField]
    private SkillTreeSlot vulnerableUnlockedButton;
    public bool VulnerableUnlocked { get; private set; }
    
    [Header("Freeze Time Skill Info")] 
    [SerializeField]
    private float freezeTimeDuration;
    
    private Vector2 playerFacingDirection;
    private readonly List<GameObject> spawnedSwords = new ();

    protected override void Start()
    {
        base.Start();
        
        swordFlyingUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockSwordFlying);
        swordBounceUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockSwordBounce);
        swordSpinUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockSwordSpin);
        timeStopUnlockedButton.GetComponent<Button>().onClick.AddListener(UnlockTimeStop);
        vulnerableUnlockedButton.GetComponent<Button>().onClick.AddListener(UnlockVulnerable);
    }

    public void CreateSword()
    {
        DestroyAllObjects();
        GameObject newSword = Instantiate(swordPrefab, player.transform.position, transform.rotation);
        spawnedSwords.Add(newSword);
        int rand = Random.Range(0, spawnedSwords.Count);
        GameObject chosenSword = spawnedSwords[rand];
        
        SwordSkillController newSwordSkillController = chosenSword.GetComponent<SwordSkillController>();

        if (swordType == SwordType.BOUNCE)
        {
            newSwordSkillController.InitializeBounce(true, amountOfBounce, bounceSpeed);
        }
        
        if (swordType == SwordType.SPIN)
        {
            newSwordSkillController.InitializeSpin(true, maxDistance, spinDuration, onHitCooldown);
        }
        
        player.NewSwordAssignment(chosenSword);
        
        newSwordSkillController.InitializeSword(player.animatorDirection, throwForce, returnSpeed, player, freezeTimeDuration);
    }
    
    private void DestroyAllObjects()
    {
        foreach (GameObject swords in spawnedSwords)
        {
            Destroy(swords); 
        }
        
        spawnedSwords.Clear(); 
    }
    
    private void UnlockTimeStop()
    {
        if (timeStopUnlockedButton.unlocked)
        {
            TimeStopUnlocked = true;
        }
    }
    
    private void UnlockVulnerable()
    {
        if (vulnerableUnlockedButton.unlocked)
        {
            VulnerableUnlocked = true;
        }
    }

    private void UnlockSwordFlying()
    {
        if (swordFlyingUnlockButton.unlocked)
        {
            swordType = SwordType.REGULAR;
            swordFlyingUnlocked = true;
        }
    }
    
    private void UnlockSwordBounce()
    {
        if (swordBounceUnlockButton.unlocked)
        {
            swordType = SwordType.BOUNCE;
        }
    }
    
    private void UnlockSwordSpin()
    {
        if (swordSpinUnlockButton.unlocked)
        {
            swordType = SwordType.SPIN;
        }
    }

    protected override void CheckUnlocked()
    {
        UnlockSwordFlying();
        UnlockSwordBounce();
        UnlockSwordSpin();
    }
}
