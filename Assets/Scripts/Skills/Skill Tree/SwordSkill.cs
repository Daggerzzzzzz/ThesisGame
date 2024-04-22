using UnityEngine;
using System.Collections.Generic;

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
    
    [Header("Sword Skill Info")]
    [SerializeField]
    private GameObject swordPrefab;
    [SerializeField] 
    private float throwForce = 15;
    [SerializeField] 
    private float returnSpeed = 15;

    [Header("Bounce Sword Skill Info")] 
    [SerializeField]
    private int amountOfBounce;
    [SerializeField]
    private int bounceSpeed;
    
    [Header("Spin Sword Skill Info")] 
    [SerializeField]
    private float maxDistance = 7;
    [SerializeField] 
    private float spinDuration = 2;
    [SerializeField] 
    private float onHitCooldown = 0.25f;

    [Header("Freeze Time Skill Info")] 
    [SerializeField]
    private float freezeTimeDuration;
    
    private Vector2 playerFacingDirection;
    private readonly List<GameObject> spawnedSwords = new ();
    
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
}
