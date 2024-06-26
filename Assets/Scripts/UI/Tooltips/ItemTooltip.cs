using TMPro;
using UnityEngine;

public class ItemTooltip : MonoBehaviour
{
    [SerializeField] 
    private TextMeshProUGUI itemNameText;
    [SerializeField] 
    private TextMeshProUGUI itemTypeText;
    [SerializeField] 
    private TextMeshProUGUI itemDescription;
    [HideInInspector]
    public RectTransform itemRectTransform;
    
    private void Awake()
    {
        itemRectTransform = GetComponent<RectTransform>();
    }

    public void ShowWeaponTooltip(WeaponDataSO weaponDataSo)
    {
        gameObject.SetActive(true);
        
        itemNameText.text = weaponDataSo.itemName.ToUpper();
        itemTypeText.text = weaponDataSo.equipmentType.ToString();
        itemDescription.text = weaponDataSo.GetDescription();
    }
    
    public void ShowArmorTooltip(ArmorDataSO armorDataSo)
    {
        gameObject.SetActive(true);
        
        itemNameText.text = armorDataSo.itemName.ToUpper();
        itemTypeText.text = armorDataSo.equipmentType.ToString();
        itemDescription.text = armorDataSo.GetDescription();
    }
    
    public void ShowItemTooltip(ItemDataSO itemDataSo)
    {
        gameObject.SetActive(true);
        
        itemNameText.text = itemDataSo.itemName.ToUpper();
        itemTypeText.text = "Stash";
        itemDescription.text = itemDataSo.GetDescription();
    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }
}
