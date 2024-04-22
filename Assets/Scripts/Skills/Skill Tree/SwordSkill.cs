using UnityEngine;
using System.Collections.Generic;

public class SwordSkill : Skill
{
    [Header("Sword Skill Info")]
    [SerializeField]
    private GameObject swordPrefab;
    [SerializeField] 
    private float throwForce = 15;
    [SerializeField] 
    private float returnSpeed = 15;

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
        
        player.NewSwordAssignment(chosenSword);
        
        newSwordSkillController.SetupSword(player.animatorDirection, throwForce, returnSpeed, player);
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
