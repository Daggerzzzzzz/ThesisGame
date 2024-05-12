using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [field:SerializeField]
    public ItemDataSO ItemDataSo { get; private set; }

    public void SetupItem(ItemDataSO _itemDataSo)
    {
        ItemDataSo = _itemDataSo;
        
        if (ItemDataSo == null)
        {
            return;
        }
        
        if (ItemDataSo.itemType == ItemType.EQUIPMENT)
        {
            EquipmentDataSO equipmentDataSo = ItemDataSo as EquipmentDataSO;
            if (equipmentDataSo != null) gameObject.name = equipmentDataSo.equipmentType.ToString().ToLower();
        }
        else
        {
            gameObject.name = ItemDataSo.name;
        }
        
        GetComponent<SpriteRenderer>().sprite = ItemDataSo.icon;
        
    }

    public void ItemPickup()
    {
        Inventory.Instance.AddItem(ItemDataSo);
        Destroy(gameObject);
    }
}
