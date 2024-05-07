using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [field:SerializeField]
    public ItemDataSO ItemDataSo { get; private set; }
    
    private void InitializeItem()
    {
        if (ItemDataSo == null)
        {
            return;
        }
        GetComponent<SpriteRenderer>().sprite = ItemDataSo.icon;
        gameObject.name = ItemDataSo.name;
    }

    public void SetupItem(ItemDataSO _itemDataSo)
    {
        ItemDataSo = _itemDataSo;
        InitializeItem();
    }

    public void ItemPickup()
    {
        Inventory.Instance.AddItem(ItemDataSo);
        Destroy(gameObject);
    }
}
