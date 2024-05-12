using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillTreeSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
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
        SetUpSkillTreeSlot();
    }

    private void UnlockSkillSlot()
    {
        if (!PlayerManager.Instance.HaveEnoughCurrency(skillCost))
        {
            return;
        }
        
        for (int i = 0; i < skillTreeSlotUnlocked.Length; i++)
        {
            if (!skillTreeSlotUnlocked[i].unlocked)
            {
                Debug.Log("Cannot Unlock Skill");
                return;
            }
        }
        
        for (int i = 0; i < skillTreeSlotLocked.Length; i++)
        {
            if (skillTreeSlotLocked[i].unlocked)
            {
                Debug.Log("Cannot Unlock Skill");
                return;
            }
        }

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
}
