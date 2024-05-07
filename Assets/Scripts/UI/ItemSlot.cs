using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour
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

    /*public void OnPointerEnter(PointerEventData eventData)
    {
        if (item.itemDataSo == null)
        {
            return;
        }
        ui.itemTooltip.ShowTooltip(item.itemDataSo);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (item.itemDataSo == null)
        {
            return;
        }
        ui.itemTooltip.HideTooltip();
    }*/
}
