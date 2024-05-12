using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class StatSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] 
    private string statName;
    [SerializeField] 
    private StatType statType;
    [SerializeField] 
    private TextMeshProUGUI statValueText;
    [SerializeField] 
    private TextMeshProUGUI statNameText;

    [TextArea] 
    [SerializeField] 
    private string StatDesc;

    private UI ui;

    private void Start()
    {
        UpdateStatValue();
        ui = GetComponentInParent<UI>();
    }

    public void UpdateStatValue()
    {
        PlayerStats playerStats = PlayerManager.Instance.player.GetComponent<PlayerStats>();
        
        if (playerStats != null)
        {
            statValueText.text = playerStats.StatToGet(statType).GetValue().ToString();

            switch (statType)
            {
                case StatType.HEALTH:
                    statValueText.text = playerStats.CalculateMaxHealthValue().ToString();
                    break;
                case StatType.DAMAGE:
                    statValueText.text = (playerStats.damage.GetValue() + playerStats.strength.GetValue()).ToString();
                    break;
                case StatType.CRITICALDAMAGE:
                    statValueText.text = (playerStats.criticalDamage.GetValue() + playerStats.strength.GetValue()).ToString();
                    break;
                case StatType.CRITCHANCE:
                    statValueText.text = (playerStats.criticalChance.GetValue() + playerStats.agility.GetValue()).ToString();
                    break;
                case StatType.DODGE:
                    statValueText.text = (playerStats.dodge.GetValue() + playerStats.agility.GetValue()).ToString();
                    break;
                case StatType.BURNDAMAGE:
                    statValueText.text = (playerStats.burnDamage.GetValue() + playerStats.intelligence.GetValue()).ToString();
                    break;
                case StatType.FREEZEDAMAGE:
                    statValueText.text = (playerStats.freezeDamage.GetValue() + playerStats.intelligence.GetValue()).ToString();
                    break;
                case StatType.SHOCKDAMAGE:
                    statValueText.text = (playerStats.shockDamage.GetValue() + playerStats.intelligence.GetValue()).ToString();
                    break;
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Vector2 mousePos;
        
        ui.statTooltip.ShowStatTooltip(StatDesc);
        
        mousePos = UIInputManager.GetMousePosition();
        
        float pivotX = mousePos.x / Screen.width;
        float pivotY = mousePos.y / Screen.height + 0.80f;

        ui.statTooltip.statRectTransform.pivot = new Vector2(pivotX, pivotY);
        ui.statTooltip.transform.position = mousePos;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.statTooltip.HideStatTooltip();
    }
}
