using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KunaiSkill : Skill
{
    [SerializeField] 
    private GameObject kunaiPrefab;
    
    [Header("Kunai Teleport")]
    [SerializeField]
    private SkillTreeSlot unlockKunaiTeleportButton;
    [field:SerializeField]
    public bool KunaiUnlocked { get; private set; }

    [Header("Kunai Switch Explode")]
    [SerializeField]
    private SkillTreeSlot unlockKunaiSwitchExplodeButton;
    [field:SerializeField]
    public bool KunaiSwitchExplodeUnlocked { get; private set; }


    [Header("Kunai Stack Explode")]
    [SerializeField]
    private SkillTreeSlot unlockKunaiStackExplodeButton;
    [field:SerializeField]
    public bool KunaiStackExplodeUnlocked { get; set; }
    
    
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
    [field:SerializeField]
    public GameObject CurrentKunai { get; private set; }

    protected override void Start()
    {
        base.Start();
        
        unlockKunaiTeleportButton.GetComponent<Button>().onClick.AddListener(UnlockKunai);
        unlockKunaiSwitchExplodeButton.GetComponent<Button>().onClick.AddListener(UnlockKunaiSwitchExplode);
        unlockKunaiStackExplodeButton.GetComponent<Button>().onClick.AddListener(UnlockKunaiStackExplode);
    }

    protected override void Update()
    {
        base.Update();
        CheckForMissingObjects(summonedKunais);
        CheckForMissingObjects(kunaiLeft);
    }

    private void UnlockKunai()
    {
        if (unlockKunaiTeleportButton.unlocked)
        {
            KunaiUnlocked = true;
        }
    }
    
    private void UnlockKunaiSwitchExplode()
    {
        if (unlockKunaiSwitchExplodeButton.unlocked)
        {
            if (SkillManager.Instance.Kunai.CurrentKunai != null)
            {
                SkillManager.Instance.Kunai.currentKunaiSkillController.KunaiDestroy();
            }
            
            KunaiSwitchExplodeUnlocked = true;
        }
    }
    
    private void UnlockKunaiStackExplode()
    {
        if (unlockKunaiStackExplodeButton.unlocked)
        {
            if (SkillManager.Instance.Kunai.CurrentKunai != null)
            {
                SkillManager.Instance.Kunai.currentKunaiSkillController.KunaiDestroy();
            }
            
            KunaiStackExplodeUnlocked = true;
        }
    }

    protected override void CheckUnlocked()
    {
        UnlockKunai();
        UnlockKunaiSwitchExplode();
        UnlockKunaiStackExplode();
    }

    protected override void UseSkill()
    {
        base.UseSkill();

        if (CanUseMultiStack())
        {
            
        }

        else if ((KunaiSwitchExplodeUnlocked || KunaiUnlocked) && !KunaiStackExplodeUnlocked)
        {
            if (CurrentKunai == null)
            {
                CurrentKunai = Instantiate(kunaiPrefab, player.transform.position, Quaternion.identity);
                currentKunaiSkillController = CurrentKunai.GetComponent<KunaiSkillController>();

                pressedTwice = false;
                
                tempCooldown = cooldown;
                cooldown = 0;
            }
            else if (CurrentKunai != null)
            {
                SoundManager.Instance.PlaySoundEffects(13, null, true);
                Vector2 playerBeforePos = player.transform.position;
        
                player.transform.position = CurrentKunai.transform.position;
                CurrentKunai.transform.position = playerBeforePos;

                pressedTwice = true;
                
                currentKunaiSkillController.SetupKunai(KunaiSwitchExplodeUnlocked, KunaiUnlocked, pressedTwice, KunaiStackExplodeUnlocked, player);
                currentKunaiSkillController.KunaiExplosion();

                cooldown = tempCooldown;
            }
        }
    }

    private bool CanUseMultiStack()
    {
        if (KunaiStackExplodeUnlocked)
        {
            if (firstEntered)
            {
                RestockKunai();
                firstEntered = false;
            }
            if (kunaiLeft.Count > 0)
            {
                cooldown = 0;
                
                GameObject kunaiToSpawn = kunaiLeft[^1];
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
            currentKunaiSkillController.SetupKunai(KunaiSwitchExplodeUnlocked, KunaiUnlocked, pressedTwice, KunaiStackExplodeUnlocked, player);
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
}
