using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KunaiSkill : Skill
{
    [SerializeField] 
    private GameObject kunaiPrefab;
    
    [Header("Teleporting Kunai")]
    [SerializeField]
    private bool canTeleport;
    
    [Header("Exploding Kunai")]
    [SerializeField]
    private bool canExplode;

    [field:Header("Multiple Kunai")]
    [field:SerializeField]
    public bool CanMultiStack { get; set; }
    [SerializeField]
    private int amountOfStacks;
    [SerializeField]
    private float multiStackCooldown;
    
    private List<GameObject> kunaiLeft = new();
    private List<GameObject> summonedKunais = new();
    
    [HideInInspector]
    public KunaiSkillController currentKunaiSkillController;

    private bool pressedTwice = false;
    [SerializeField]
    private bool firstEntered = true;
    
    public GameObject CurrentKunai { get; private set; }
    
    protected override void Update()
    {
        base.Update();
        CheckForMissingObjects(summonedKunais);
        CheckForMissingObjects(kunaiLeft);
    }

    public override void UseSkill()
    {
        base.UseSkill();

        if (CanUseMultiStack())
        {
            return;
        }

        if (canExplode || canTeleport)
        {
            if (CurrentKunai == null)
            {
                CurrentKunai = Instantiate(kunaiPrefab, player.transform.position, Quaternion.identity);
                currentKunaiSkillController = CurrentKunai.GetComponent<KunaiSkillController>();
                
                pressedTwice = false;
                player.OnPlayerInputs.Player.Teleport.Enable();
            }
            else
            {
                player.OnPlayerInputs.Player.Teleport.Disable();
                Vector2 playerBeforePos = player.transform.position;
            
                player.transform.position = CurrentKunai.transform.position;
                CurrentKunai.transform.position = playerBeforePos;

                pressedTwice = true;
                currentKunaiSkillController.SetupCrystal(canExplode, canTeleport, pressedTwice, CanMultiStack);
                currentKunaiSkillController.KunaiExplosion();
                StartCoroutine(ExplosionAnimationDelay());
            }
        }
    }

    private bool CanUseMultiStack()
    {
        if (CanMultiStack)
        {
            if (firstEntered)
            {
                RestockKunai();
                firstEntered = false;
            }
            player.OnPlayerInputs.Player.Teleport.Enable();
            if (kunaiLeft.Count > 0)
            {
                cooldown = 0;
                
                GameObject kunaiToSpawn = kunaiLeft[kunaiLeft.Count - 1];
                GameObject newKunai = Instantiate(kunaiToSpawn, player.transform.position, Quaternion.identity);
                summonedKunais.Add(newKunai);
                kunaiLeft.Remove(kunaiToSpawn);

                if (kunaiLeft.Count <= 0)
                {
                    return false;
                }
                
                return true;
            }

            if (kunaiLeft.Count <= 0)
            {
                pressedTwice = true;
                cooldown = multiStackCooldown;
                DeleteAllKunais();
                RestockKunai();
                pressedTwice = false;
            }
        }
        return false;
    }

    private void DeleteAllKunais()
    {
        foreach (GameObject kunai in summonedKunais)
        {
            currentKunaiSkillController = kunai.GetComponent<KunaiSkillController>();
            currentKunaiSkillController.SetupCrystal(canExplode, canTeleport, pressedTwice, CanMultiStack);
        }
    }

    private void RestockKunai()
    {
        int tempAmount = amountOfStacks - kunaiLeft.Count;

        if (firstEntered)
        {
            for (int i = 0; i < 3; i++)
            {
                kunaiLeft.Add(kunaiPrefab);
            }
        }
        else
        {
            for (int i = 0; i < tempAmount; i++)
            {
                kunaiLeft.Add(kunaiPrefab);
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
    
    private IEnumerator ExplosionAnimationDelay()
    {
        yield return new WaitForSeconds(1);
        player.OnPlayerInputs.Player.Teleport.Enable();
    }
}
