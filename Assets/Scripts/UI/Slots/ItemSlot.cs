using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private Image itemImage;
    [SerializeField]
    private TextMeshProUGUI itemText;

    public InventoryItem item;

    private UI ui;

    private void Start()
    {
        ui = GetComponentInParent<UI>();
    }

    public void UpdateSlot(InventoryItem newItem)
    {
        item = newItem;
        itemImage.color = Color.white;
        
        if (item != null)
        {
            itemImage.sprite = item.itemDataSo.icon;
            
            if (item.stackSize > 1)
            {
                itemText.text = item.stackSize.ToString();
            }
            else
            {
                itemText.text = "";
            }
        }
    }
    
    public void CleanUpSlot()
    {
        item = null;

        itemImage.sprite = null;
        itemImage.color = Color.clear;
        itemText.text = "";
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Vector2 mousePos;

        if (item.itemDataSo != null)
        {
            if (item.itemDataSo.itemType == ItemType.EQUIPMENT)
            {
                EquipmentDataSO equipmentDataSo = item.itemDataSo as EquipmentDataSO;

                if (equipmentDataSo != null && equipmentDataSo.equipmentType == EquipmentType.ARMOR)
                {
                    ArmorDataSO armorDataSo = equipmentDataSo as ArmorDataSO;
                    ui.itemTooltip.ShowArmorTooltip(armorDataSo);
                }
                else if (equipmentDataSo != null && equipmentDataSo.equipmentType == EquipmentType.WEAPON)
                {
                    WeaponDataSO weaponDataSo = equipmentDataSo as WeaponDataSO;
                    ui.itemTooltip.ShowWeaponTooltip(weaponDataSo);
                }
            }
            else if (item.itemDataSo.itemType == ItemType.MATERIAL)
            {
                ui.itemTooltip.ShowItemTooltip(item.itemDataSo);
            }

            mousePos = UIInputManager.GetMousePosition();

            float pivotX = mousePos.x / Screen.width;
            float pivotY = mousePos.y / Screen.height + 0.80f;

            ui.itemTooltip.itemRectTransform.pivot = new Vector2(pivotX, pivotY);

            ui.itemTooltip.transform.position = mousePos;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (item == null)
        {
            return;
        }
        ui.itemTooltip.HideTooltip();
    }
}
