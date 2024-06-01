using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillTreeSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISaveManager
{
    [SerializeField] 
    private SkillTreeSlot[] skillTreeSlotUnlocked;
    [SerializeField] 
    private SkillTreeSlot[] skillTreeSlotLocked;
    [SerializeField] 
    private string skillName;
    [SerializeField] 
    private int skillCost;
    [SerializeField] 
    private Color lockedSkillColor;

    [TextArea] 
    [SerializeField] 
    private string skillDesc;

    public bool unlocked;
    private Image skillImage;

    private UI ui;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(UnlockSkillSlot);
    }

    private void Start()
    {
        ui = GetComponentInParent<UI>();
        skillImage = GetComponent<Image>();
        
        skillImage.color = lockedSkillColor;

        if (unlocked)
        {
            skillImage.color = Color.white;
        }
        
        SetUpSkillTreeSlot();
    }

    private void UnlockSkillSlot()
    {
        if (!PlayerManager.Instance.HaveEnoughCurrency(skillCost))
        {
            SoundManager.Instance.PlaySoundEffects(32, null, false);
            return;
        }
        
        for (int i = 0; i < skillTreeSlotUnlocked.Length; i++)
        {
            if (!skillTreeSlotUnlocked[i].unlocked)
            {
                SoundManager.Instance.PlaySoundEffects(32, null, false);
                Debug.Log("Cannot Unlock Skill");
                return;
            }
        }
        
        for (int i = 0; i < skillTreeSlotLocked.Length; i++)
        {
            if (skillTreeSlotLocked[i].unlocked)
            {
                SoundManager.Instance.PlaySoundEffects(32, null, false);
                Debug.Log("Cannot Unlock Skill");
                return;
            }
        }

        SoundManager.Instance.PlaySoundEffects(19, null, false);
        unlocked = true;
        skillImage.color = Color.white;
    }

    private void SetUpSkillTreeSlot()
    {
        gameObject.name = "Skill Tree Slot " + skillName;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Vector2 mousePos;
        
        ui.skillTooltip.ShowTooltip(skillName, skillDesc);

        mousePos = UIInputManager.GetMousePosition();

        float pivotX = mousePos.x / Screen.width;
        float pivotY = mousePos.y / Screen.height + 0.80f;

        ui.skillTooltip.skillRectTransform.pivot = new Vector2(pivotX, pivotY);
        ui.skillTooltip.transform.position = mousePos;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.skillTooltip.HideTooltip();
    }

    public void LoadData(GameData data)
    {
        if (data.skillTree.TryGetValue(skillName, out bool value))
        {
            unlocked = value;
        }
    }

    public void SaveData(ref GameData data)
    {
        if (data.skillTree.TryGetValue(skillName, out bool value))
        {
            data.skillTree.Remove(skillName);
            data.skillTree.Add(skillName, unlocked);
        }
        else
        {
            data.skillTree.Add(skillName, unlocked);
        }
    }
}
